using System.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.NintenTools.Byaml.Dynamic;

namespace Nintendo
{
    public static class ByamlUtil
    {
        public static dynamic Load(string filePath)
        {
            return Load(File.ReadAllBytes(filePath));
        }

        public static dynamic Load(byte[] rawByaml)
        {
            using (MemoryStream memoryStream = new MemoryStream(rawByaml))
            {
                return Load(memoryStream);
            }
        }

        public static dynamic Load(System.IO.Stream stream)
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