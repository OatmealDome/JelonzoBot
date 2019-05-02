using System.IO;
using System.Text;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using JelonzoBot.Core.Config;
using LibHac.IO;
using Nintendo.Blitz;
using Nintendo.Switch;
using Syroot.BinaryData;
using Syroot.NintenTools.Byaml.Dynamic;
using Syroot.NintenTools.Yaz0;

namespace JelonzoBot.Blitz
{
    public static class RomResourceLoader
    {
        private static byte[] Yaz0MagicNumbers = Encoding.ASCII.GetBytes("Yaz0");
        private static NcaWrapper NcaWrapper;
        
        public static void Initialize()
        {
            // Get the RomConfig
            RomConfig romConfig = ((JelonzoBotConfiguration)Configuration.LoadedConfiguration).RomConfig;

            // Load the base and update NCAs
            NcaWrapper = new NcaWrapper(KeysetManager.GetKeyset(), romConfig.BaseNcaPath, romConfig.UpdateNcaPath);
        }

        public static void Dispose()
        {
            // Dispose of the NcaWrapper
            NcaWrapper.Dispose();
        }

        public static byte[] GetFile(string romPath)
        {
            // Load the file
            IFile file = NcaWrapper.Romfs.OpenFile(romPath, OpenMode.Read);

            // Get the file as a Stream
            Stream stream = file.AsStream();

            // Check if this is a nisasyst file
            if (Nisasyst.IsNisasystFile(stream))
            {
                // Get the game path for Nisasyst
                string gamePath = romPath.StartsWith('/') ? romPath.Substring(1) : romPath;

                // Create a new MemoryStream
                MemoryStream memoryStream = new MemoryStream();

                // Decrypt the file
                Nisasyst.Decrypt(stream, memoryStream, gamePath);

                // Switch the streams
                stream.Dispose();
                stream = memoryStream;
            }

            // Create a buffer stream and a place to store the final data
            MemoryStream bufferStream = new MemoryStream();
            byte[] finalData;

            try
            {
                // Attempt to Yaz0 decompress this file
                Yaz0Compression.Decompress(stream, bufferStream);

                // Set the decompresed file
                finalData = bufferStream.ToArray();
            }
            catch (Yaz0Exception)
            {
                // Not a Yaz0 file, so just copy the stream into the byte array
                stream.Seek(0, SeekOrigin.Begin);
                finalData = new byte[stream.Length];
                stream.Read(finalData, 0, finalData.Length);
            }

            // Dispose of the buffer stream
            bufferStream.Dispose();

            // Dispose of the input stream
            stream.Dispose();

            // Return the array
            return finalData;
        }

        public static dynamic GetLocalByaml(byte[] ramByaml)
        {
            // Load the BYAML from a MemoryStream
            using (MemoryStream stream = new MemoryStream(ramByaml))
            {
                return LoadByaml(stream);
            }
        }

        public static dynamic GetRomByaml(string romPath)
        {
            // Load the BYAML from ROM
            using (MemoryStream stream = new MemoryStream(GetFile(romPath)))
            {
                return LoadByaml(stream);
            }
        }

        private static dynamic LoadByaml(Stream stream)
        {
            // Decide the course of action
            ushort firstBytes;
            using (BinaryDataReader reader = new BinaryDataReader(stream, Encoding.ASCII, true))
            using (BinaryDataWriter writer = new BinaryDataWriter(stream, Encoding.ASCII, true))
            {
                // Read the first two bytes of the file in big endian
                reader.ByteOrder = ByteOrder.BigEndian;
                firstBytes = reader.ReadUInt16();

                // Check endianness
                if (firstBytes == 0x4259) // "BY" (big endian)
                {
                    reader.ByteOrder = ByteOrder.BigEndian;
                }
                else if (firstBytes == 0x5942) // "YB" (little endian)
                {
                    reader.ByteOrder = ByteOrder.LittleEndian;
                }
                else // Not a byaml?
                {
                    return null;
                }

                // Force version to 1
                writer.Seek(0x2, SeekOrigin.Begin);
                writer.Write((ushort)0x0001);

                // Get the first byte at where the path array offset is
                reader.Seek(0x10, SeekOrigin.Begin);
                byte firstByte = reader.ReadByte();

                // Seek back to the beginning
                stream.Seek(0, SeekOrigin.Begin);

                // Load the byaml
                return ByamlFile.Load(stream, firstByte == 0xc3, reader.ByteOrder);
            }
        }

    }
}