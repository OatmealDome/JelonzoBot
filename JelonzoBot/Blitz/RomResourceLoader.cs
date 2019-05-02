using System.IO;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using JelonzoBot.Core.Config;
using LibHac.IO;
using Nintendo.Blitz;
using Nintendo.Switch;

namespace JelonzoBot.Blitz
{
    public static class RomResourceLoader
    {
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

                // Decrypt the file
                return Nisasyst.Decrypt(stream, gamePath);
            }

            // Read the file to a byte array
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);

            // Return the array
            return data;
        }

    }
}