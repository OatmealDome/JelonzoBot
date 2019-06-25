using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using JelonzoBot.Core.Config;
using LibHac.IO;
using Nintendo.Archive;
using Nintendo.Blitz;
using Nintendo.Switch;
using Syroot.BinaryData;
using Syroot.NintenTools.Byaml.Dynamic;
using Syroot.NintenTools.Byaml.Serialization;
using Syroot.NintenTools.Yaz0;

namespace JelonzoBot.Blitz
{
    public static class RomResourceLoader
    {
        private static byte[] Yaz0MagicNumbers = Encoding.ASCII.GetBytes("Yaz0");
        private static NcaWrapper NcaWrapper;
        private static Dictionary<string, byte[]> PackFiles;
        
        public static void Initialize()
        {
            // Get the RomConfig
            RomConfig romConfig = ((JelonzoBotConfiguration)Configuration.LoadedConfiguration).RomConfig;

            // Load the base and update NCAs
            NcaWrapper = new NcaWrapper(KeysetManager.GetKeyset(), romConfig.BaseNcaPath, romConfig.UpdateNcaPath);

            // Create a new pack files  dictionary
            PackFiles = new Dictionary<string, byte[]>();

            // Load the pack directory
            IDirectory packDirectory = NcaWrapper.Romfs.OpenDirectory("/Pack", OpenDirectoryMode.Files);
            
            // Read every directory entries
            foreach (DirectoryEntry directoryEntry in packDirectory.Read())
            {
                // Load the SARC
                Sarc sarc = new Sarc(GetRomFileFromRomfs(directoryEntry.FullPath));

                // Loop over every file
                foreach (KeyValuePair<string, byte[]> pair in sarc)
                {
                    // Add this file to the pack files dictionary
                    PackFiles.Add("/" + pair.Key, pair.Value);
                }
            }
        }

        public static void Dispose()
        {
            // Dispose of the NcaWrapper
            NcaWrapper.Dispose();

            // Dispose the pack files
            PackFiles = null;
        }

        public static Stream GetRomFile(string romPath)
        {
            // Attempt to load from the packs first
            if (PackFiles.TryGetValue(romPath, out byte[] file))
            {
                // Return a MemoryStream
                return new MemoryStream(file);
            }
            else
            {
                // Load from the romfs
                return GetRomFileFromRomfs(romPath);
            }
        }

        public static Stream GetRomFileFromRomfs(string romPath)
        {
            // Load the file
            IFile file = NcaWrapper.Romfs.OpenFile(romPath, OpenMode.Read);

            // Get the file as a Stream
            Stream stream = file.AsStream();

            // Check if this is a nisasyst file
            // .pack files are automatically considered to be not nisasyst-encrypted,
            // since a nisasyst-encrypted file can be placed at the end and trip up
            // the check. Let's hope that a pack doesn't become nisasyst encrypted.
            if (Path.GetExtension(romPath) != ".pack" && Nisasyst.IsNisasystFile(stream))
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

            // Read the first four bytes
            byte[] firstBytes = new byte[4];
            stream.Read(firstBytes, 0, 4);
            stream.Seek(0, SeekOrigin.Begin);

            // Check if the file is Yaz0 compressed
            if (firstBytes.SequenceEqual(Yaz0MagicNumbers))
            {
                // Create a new MemoryStream
                MemoryStream bufferStream = new MemoryStream();

                // Attempt to Yaz0 decompress this file
                Yaz0Compression.Decompress(stream, bufferStream);

                // Seek to the beginning
                bufferStream.Seek(0, SeekOrigin.Begin);

                // Switch the streams
                stream.Dispose();
                stream = bufferStream;
            }

            // Return the stream
            return stream;
        }

    }
}