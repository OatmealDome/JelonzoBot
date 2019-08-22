using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Scheduler;
using BcatBotFramework.Social.Discord;
using JelonzoBot.Blitz.Internationalization;
using JelonzoBot.Core.Config;
using JelonzoBot.Scheduler.Job;
using LibHac.IO;
using Newtonsoft.Json;
using Nintendo.Archive;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Switch;
using Quartz;
using Syroot.NintenTools.Yaz0;

namespace JelonzoBot.Blitz
{
    public static class RomResourceLoader
    {
        private static byte[] Yaz0MagicNumbers = Encoding.ASCII.GetBytes("Yaz0");

        private static NcaWrapper NcaWrapper;
        private static Dictionary<string, byte[]> PackFiles;
        
        public static async Task Initialize()
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

            // Load GameConfigSetting
            XDocument gameConfig = XDocument.Load(GetRomFile("/System/GameConfigSetting.xml"));

            // Get the application version
            int appVersion = int.Parse(gameConfig.Root
                .Elements("category").Where(e => e.Attribute("name").Value == "Root").First()
                .Elements("category").Where(e => e.Attribute("name").Value == "Project").First()
                .Elements("category").Where(e => e.Attribute("name").Value == "Version").First()
                .Elements("parameter").Where(e => e.Attribute("name").Value == "AppVersion").First()
                .Attribute("defaultValue").Value);

            // Output the ROM version
            await DiscordBot.LoggingChannel.SendMessageAsync($"**[RomResourceLoader]** ROM version {appVersion} loaded");

            // Check if this version is new compared to the last boot
            if (romConfig.LastRomVersion < appVersion)
            {
                // Upload necessary ROM data after everything is initalized
                // TODO: bad hack - what if initialization takes >1min? (unlikely but not impossible)
                await QuartzScheduler.ScheduleJob<RomDataUploadJob>("Normal", DateTime.Now.AddMinutes(1), new JobDataMap());

                // Set the last app version
                romConfig.LastRomVersion = appVersion;

                // Save the configuration
                Configuration.LoadedConfiguration.Write();
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
            Stream stream;
            if (PackFiles.TryGetValue(romPath, out byte[] file))
            {
                // Create a MemoryStream
                stream = new MemoryStream(file);
            }
            else
            {
                // Load from the romfs
                stream = GetRomFileFromRomfs(romPath);
            }

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

        public static Stream GetRomFileFromRomfs(string romPath)
        {
            // Load the file
            IFile file = NcaWrapper.Romfs.OpenFile(romPath, OpenMode.Read);

            // Get the file as a Stream
            return file.AsStream();
        }

        public static IList<string> GetFilesInDirectory(string path)
        {
            // Create a list
            List<string> filesList = new List<string>();

            if (NcaWrapper.Romfs.DirectoryExists(path))
            {
                // Load the pack directory
                IDirectory packDirectory = NcaWrapper.Romfs.OpenDirectory(path, OpenDirectoryMode.Files);
                
                // Read every directory entries
                foreach (DirectoryEntry directoryEntry in packDirectory.Read())
                {
                    // Add this file to the list
                    filesList.Add(directoryEntry.FullPath);
                }
            }

            // Get the number of slashes that should be in the path
            int slashCount = path.Where(c => c == '/').Count() + (path.EndsWith('/') ? 0 : 1);

            // Go through every pack file to see if there's anything that matches
            foreach (string packPath in PackFiles.Keys)
            {
                // Check if the path starts with the search path and that the slash count is equal
                // If the slash count is equal, then the file is in the directory and not within
                // a subdirectory.
                if (packPath.StartsWith(path) && packPath.Where(c => c == '/').Count() == slashCount)
                {
                    // Add this file to the list
                    filesList.Add(packPath);
                }
            }

            // Check if there's any matching 
            return filesList;
        }

    }
}