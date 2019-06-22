using System;
using System.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.NintenTools.Byaml.Dynamic;
using Syroot.NintenTools.Byaml.Serialization;

namespace JelonzoBot.Blitz
{
    public static class ByamlLoader
    {
        private struct ByamlSettings
        {
            public ByteOrder byteOrder;
            public bool supportsPaths;
        }
        
        public static dynamic GetByamlDynamic(byte[] ramByaml)
        {
            // Load the BYAML from a MemoryStream
            using (MemoryStream stream = new MemoryStream(ramByaml))
            {
                // Get the byaml settings
                ByamlSettings settings = PrepareByaml(stream);

                // Read the byaml
                return LoadByamlDynamic(settings, stream);
            }
        }

        public static dynamic GetByamlDynamic(string romPath)
        {
            using (Stream stream = RomResourceLoader.GetRomFile(romPath))
            {
                // Get the byaml settings
                ByamlSettings settings = PrepareByaml(stream);

                // Read the byaml
                return LoadByamlDynamic(settings, stream);
            }
        }

        public static T GetByamlDeserialized<T>(byte[] ramByaml)
        {
            // Load the BYAML from a MemoryStream
            using (MemoryStream stream = new MemoryStream(ramByaml))
            {
                // Get the byaml settings
                ByamlSettings settings = PrepareByaml(stream);

                // Read the byaml
                return LoadByamlSerialized<T>(settings, stream);
            }
        }

        public static T GetByamlDeserialized<T>(string romPath)
        {
            using (Stream stream = RomResourceLoader.GetRomFile(romPath))
            {
                // Get the byaml settings
                ByamlSettings settings = PrepareByaml(stream);

                // Read the byaml
                return LoadByamlSerialized<T>(settings, stream);
            }
        }

        private static ByamlSettings PrepareByaml(Stream stream)
        {
            using (BinaryDataReader reader = new BinaryDataReader(stream, Encoding.ASCII, true))
            {
                // Create a new ByamlSettings instance
                ByamlSettings byamlSettings = new ByamlSettings();

                // Read the first two bytes of the file in big endian
                reader.ByteOrder = ByteOrder.BigEndian;
                string byamlMagic = reader.ReadString(2);

                // Check endianness
                if (byamlMagic == "BY") // "BY" (big endian)
                {
                    byamlSettings.byteOrder = ByteOrder.BigEndian;
                }
                else if (byamlMagic == "YB") // "YB" (little endian)
                {
                    byamlSettings.byteOrder = ByteOrder.LittleEndian;
                }
                else // Not a byaml?
                {
                    throw new Exception("Not a BYAML file");
                }

                // Get the first byte at where the path array offset is
                reader.Seek(0x10, SeekOrigin.Begin);
                byamlSettings.supportsPaths = reader.ReadByte() == 0xc3;

                // Seek back to the beginning
                stream.Seek(0, SeekOrigin.Begin);

                // Return the settings
                return byamlSettings;
            }
        }

        private static dynamic LoadByamlDynamic(ByamlSettings settings, Stream stream)
        {
            return ByamlFile.Load(stream, settings.supportsPaths, settings.byteOrder);
        }

        private static T LoadByamlSerialized<T>(ByamlSettings settings, Stream stream)
        {
            // Create a ByamlSerializer instance
            ByamlSerializer serializer = new ByamlSerializer(new ByamlSerializerSettings()
            {
                ByteOrder = settings.byteOrder,
                SupportPaths = settings.supportsPaths,
                Version = ByamlVersion.Version1 // unused, version is ignored
            });

            // Deserialize the byaml
            return serializer.Deserialize<T>(stream);
        }

    }
}