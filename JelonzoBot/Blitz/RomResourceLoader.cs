using System;
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
        private struct ByamlSettings
        {
            public ByteOrder byteOrder;
            public bool supportsPaths;
        }

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

        public static Stream GetRomFile(string romPath)
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

            try
            {
                // Create a new MemoryStream
                MemoryStream bufferStream = new MemoryStream();

                // Attempt to Yaz0 decompress this file
                Yaz0Compression.Decompress(stream, bufferStream);

                // Switch the streams
                stream.Dispose();
                stream = bufferStream;
            }
            catch (Yaz0Exception)
            {
                // Seek back to the beginning
                stream.Seek(0, SeekOrigin.Begin);
            }

            // Return the stream
            return stream;
        }

        public static dynamic GetByamlDynamicFromLocal(byte[] ramByaml)
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

        public static dynamic GetByamlDynamicFromRom(string romPath)
        {
            using (Stream stream = GetRomFile(romPath))
            {
                // Get the byaml settings
                ByamlSettings settings = PrepareByaml(stream);

                // Read the byaml
                return LoadByamlDynamic(settings, stream);
            }
        }

        private static ByamlSettings PrepareByaml(Stream stream)
        {
            using (BinaryDataReader reader = new BinaryDataReader(stream, Encoding.ASCII, true))
            using (BinaryDataWriter writer = new BinaryDataWriter(stream, Encoding.ASCII, true))
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

    }
}