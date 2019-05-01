using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nintendo.Bcat;
using Discord.WebSocket;
using MessagePack;
using Quartz;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Social.Discord;
using BcatBotFramework.Difference;
using BcatBotFramework.Core;
using Nintendo.Blitz;
using JelonzoBot.Core;
using JelonzoBot.Difference;
using Syroot.NintenTools.Byaml.Dynamic;
using Syroot.BinaryData;
using BcatBotFramework.Scheduler;
using JelonzoBot.Core.Config;
using Nintendo;

namespace JelonzoBot.Scheduler.Job
{
    public class BcatFirstRunJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (Configuration.LoadedConfiguration.FirstRunCompleted)
                {
                    throw new Exception("Attempting to do first run more than once");
                }

                foreach (KeyValuePair<RomType, BcatPair> bcatPairEntry in Program.BcatPairs)
                {
                    // Log that we're about to begin a check
                    await DiscordBot.LoggingChannel.SendMessageAsync("**[BCAT]** Beginning topic download for " + bcatPairEntry.Key.ToString());

                    // Download the latest Topic
                    Topic topic = await BcatApi.GetDataTopic(bcatPairEntry.Value.TitleId, bcatPairEntry.Value.Passphrase);

                    // Create the target folder name
                    string targetFolder = string.Format(Program.LOCAL_OLD_DATA_DIRECTORY, DateTime.Now.ToString(Program.FOLDER_DATE_TIME_FORMAT), bcatPairEntry.Key.ToString());

                    // Download all data
                    Dictionary<string, byte[]> downloadedData = await BcatCheckerUtils.DownloadAllData(topic, bcatPairEntry.Value.TitleId, bcatPairEntry.Value.Passphrase, targetFolder);

                    // Loop over all data
                    foreach (KeyValuePair<string, byte[]> dataPair in downloadedData)
                    {
                        // Get the FileType
                        FileType fileType = FileTypeExtensions.GetTypeFromFilePath(dataPair.Key);

                        // Populate the FileCache directories based on the FileType
                        string path;
                        switch (fileType)
                        {
                            case FileType.VersusSetting:
                                path = string.Format(FileCache.VERSUS_SETTING_PATH, bcatPairEntry.Key.ToString());
                                break;
                            case FileType.CoopSetting:
                                // Only write the CoopSetting file once
                                if (bcatPairEntry.Key != RomType.NorthAmerica)
                                {
                                    continue;
                                }

                                path = FileCache.COOP_SETTING_PATH;

                                break;
                            case FileType.FestivalByaml:
                                // Deserialize the byaml to get the ID
                                dynamic byaml = ByamlUtil.Load(dataPair.Value);

                                // Generate the path
                                path = string.Format(FileCache.FESTIVAL_SETTING_PATH, bcatPairEntry.Key.ToString(), byaml["FestivalId"]);

                                break;
                            default:
                                continue;
                        }

                        // Create the directories if needed
                        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));

                        // Write the file
                        File.WriteAllBytes(path, dataPair.Value);
                    }

                    // Write out the topic
                    File.WriteAllBytes(string.Format(Program.LOCAL_LAST_TOPIC, bcatPairEntry.Key.ToString()), MessagePackSerializer.Serialize(topic));

                    // Set the last data download directory
                    (Configuration.LoadedConfiguration as JelonzoBotConfiguration).LastDownloadPaths[bcatPairEntry.Key] = targetFolder;

                    // Log that the first run is done
                    await DiscordBot.LoggingChannel.SendMessageAsync($"**[BCAT]** First run complete for {bcatPairEntry.Key.ToString()}");
                }

                // Save the configuration
                Configuration.LoadedConfiguration.FirstRunCompleted = true;
                Configuration.LoadedConfiguration.Write();

                // Initialize the FileCache
                FileCache.Initialize();
            }
            catch (Exception exception)
            {
                // Notify the logging channel
                await DiscordUtil.HandleException(exception, $"in ``BcatCheckerJob``");
            }

            await QuartzScheduler.ScheduleJob<BcatCheckerJob>("Normal", Configuration.LoadedConfiguration.JobSchedules["Bcat"]);
        }

    }
}