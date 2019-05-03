using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syroot.BinaryData;

namespace Nintendo.Archive
{
    public class Sarc : IEnumerable<KeyValuePair<string, byte[]>>
    {
        struct SfatNode
        {
            public uint Hash;
            public bool HasName;
            public int NameOfs;
            public uint DataOfs;
            public uint DataLength;
        }

        private Dictionary<string, byte[]> Files;

        public byte[] this[string key]
        {
            get
            {
                return Files[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Sarc(byte[] rawSarc)
        {
            using (MemoryStream memoryStream = new MemoryStream(rawSarc))
            {
                Read(memoryStream);
            }
        }

        public Sarc(Stream stream)
        {
            Read(stream);
        }

        private void Read(Stream stream)
        {
            // Create a dictionary to hold the files
            Files = new Dictionary<string, byte[]>();

            using (BinaryDataReader reader = new BinaryDataReader(stream, true))
            {
                // Set endianness to big by default
                reader.ByteOrder = ByteOrder.BigEndian;

                // Verify the magic numbers
                if (reader.ReadString(4) != "SARC")
                {
                    throw new Exception("Not a SARC file");
                }

                // Skip the header length
                reader.Seek(2);

                // Check the byte order mark to see if this file is little endian
                if (reader.ReadUInt16() == 0xFFFE)
                {
                    // Set the endiannes to little
                    reader.ByteOrder = ByteOrder.LittleEndian;
                }

                // Check the file length
                if (reader.ReadUInt32() != reader.Length)
                {
                    throw new Exception("SARC is possibly corrupt, invalid length");
                }

                // Read the beginning of data offset
                uint dataBeginOfs = reader.ReadUInt32();

                // Verify the version
                if (reader.ReadUInt16() != 0x0100)
                {
                    throw new Exception("Unsupported SARC version");
                }

                // Seek past the reserved area
                reader.Seek(2);

                // Verify the SFAT magic numbers
                if (reader.ReadString(4) != "SFAT")
                {
                    throw new Exception("Could not find SFAT section");
                }

                // Skip the header length
                reader.Seek(2);

                // Read the node count and hash key
                ushort nodeCount = reader.ReadUInt16();
                uint hashKey = reader.ReadUInt32();

                // Read every node
                List<SfatNode> nodes = new List<SfatNode>();
                for (ushort i = 0; i < nodeCount; i++)
                {
                    // Read the node details
                    uint hash = reader.ReadUInt32();
                    uint fileAttrs = reader.ReadUInt32();
                    uint nodeDataBeginOfs = reader.ReadUInt32();
                    uint nodeDataEndOfs = reader.ReadUInt32();

                    // Create a new SfatNode
                    nodes.Add(new SfatNode()
                    {
                        Hash = hash,
                        HasName = (fileAttrs & 0x01000000) == 0x01000000, // check for name flag
                        NameOfs = (int)(fileAttrs & 0x0000FFFF) * 4, // mask upper bits and multiply by 4
                        DataOfs = nodeDataBeginOfs,
                        DataLength = nodeDataEndOfs - nodeDataBeginOfs
                    });
                }

                // Verify the SFNT magic numbers
                if (reader.ReadString(4) != "SFNT")
                {
                    throw new Exception("Could not find SFNT section");
                }

                // SKip header length and reserved area
                reader.Seek(4);

                // Get the file name beginning offset
                long nameBeginOfs = reader.Position;

                // Read each file using its SfatNode
                foreach (SfatNode node in nodes)
                {
                    // Read the filename
                    string filename;

                    // Check if there is a name offset
                    if (node.HasName)
                    {
                        // Read the name at this position
                        using (reader.TemporarySeek(nameBeginOfs + node.NameOfs, SeekOrigin.Begin))
                        {
                            filename = reader.ReadString(BinaryStringFormat.ZeroTerminated);
                        }
                    }
                    else
                    {
                        // Use the hash as the name
                        filename = node.Hash.ToString("X8") + ".bin";
                    }

                    // Read the file data
                    byte[] fileData;
                    using (reader.TemporarySeek(dataBeginOfs + node.DataOfs, SeekOrigin.Begin))
                    {
                        fileData = reader.ReadBytes((int)node.DataLength);
                    }

                    // Add the file to the dictionary
                    Files.Add(filename, fileData);
                }
            }
        }

        public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
        {
            return Files.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
}