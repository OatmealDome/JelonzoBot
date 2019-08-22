using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Social.Discord;
using DigitalOcean;
using DigitalOcean.Cdn;
using JelonzoBot.Blitz;
using JelonzoBot.Blitz.Internationalization;
using Newtonsoft.Json;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Quartz;
using S3;

namespace JelonzoBot.Scheduler.Job
{
    public class RomDataUploadJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Log that we're about to begin uploading data
                await DiscordBot.LoggingChannel.SendMessageAsync("**[RomDataUploadJob]** Beginning ROM data upload");

                // Create a list of paths whose cache needs to be cleared
                List<string> clearPaths = new List<string>();

                // Create the MSBT S3 path format string
                string msbtS3BasePath = "/splatoon/blitz_rom/Message/CommonMsg_{0}";

                // Loop over every language
                foreach (Language language in BlitzUtil.SupportedLanguages)
                {
                    // Log the language
                    await DiscordBot.LoggingChannel.SendMessageAsync($"**[RomDataUploadJob]** Uploading {language.ToString()}'s MSBTs");

                    // Get the MsbtHolder
                    MsbtHolder msbtHolder = BlitzLocalizer.MsbtHolders[language];

                    // Loop over every MSBT
                    foreach (KeyValuePair<string, Dictionary<string, string>> pair in msbtHolder.Msbts)
                    {
                        // Construct the path
                        string msbtPath = string.Format(msbtS3BasePath, language.GetSeadCode());

                        // Construct the file name
                        string fileName = $"{pair.Key}.json";

                        // Serialize to JSON
                        string json = JsonConvert.SerializeObject(pair.Value);
                        
                        // Upload to S3
                        //S3Api.TransferFile(Encoding.UTF8.GetBytes(json), msbtPath, fileName, "application/json");

                        // Add to the cache list
                        clearPaths.Add($"{msbtPath}/{fileName}");
                    }
                }

                // Log MSBT upload
                await DiscordBot.LoggingChannel.SendMessageAsync("**[RomDataUploadJob]** Uploading Mush");

                // Create the Mush S3 path string
                string mushS3Path = "/splatoon/blitz_rom/Mush";

                // Get every file in Mush
                foreach (string path in RomResourceLoader.GetFilesInDirectory("/Mush"))
                {
                    // Get the BYAML
                    dynamic byaml = ByamlLoader.GetByamlDynamic(path);

                    // Construct the file name
                    string fileName =  $"{Path.GetFileNameWithoutExtension(path)}.json";

                    // Serialize to JSON
                    string json = JsonConvert.SerializeObject(byaml);
                        
                    // Upload to S3
                    //S3Api.TransferFile(Encoding.UTF8.GetBytes(json), mushS3Path, fileName, "application/json");

                    // Add to the paths to clear
                    clearPaths.Add($"{mushS3Path}/{fileName}");
                }

                // Log CDN cache purge starting
                await DiscordBot.LoggingChannel.SendMessageAsync($"**[RomDataUploadJob]** Requesting CDN cache purge ({clearPaths.Count} of {clearPaths.Count} files left)");
                
                IEnumerable<string> pathsEnumerable = (IEnumerable<string>)clearPaths;

                while (pathsEnumerable.Any())
                {
                    // Tell DigitalOcean to clear the cache
                    await DoApi.SendRequest(new DoCdnCachePurgeRequest(Configuration.LoadedConfiguration.DoConfig.EndpointId, pathsEnumerable.Take(15).ToList()));

                    // Advance to the next set
                    pathsEnumerable = pathsEnumerable.Skip(15);

                    // Log files left
                    await DiscordBot.LoggingChannel.SendMessageAsync($"**[RomDataUploadJob]** Requesting CDN cache purge ({pathsEnumerable.Count()} of {clearPaths.Count} files left)");
                }

                // Log that it's complete
                await DiscordBot.LoggingChannel.SendMessageAsync("**[RomDataUploadJob]** ROM data upload complete");
            }
            catch (Exception e)
            {
                await DiscordUtil.HandleException(e, "in ``RomDataUploadJob``");
            }
        }

    }
}