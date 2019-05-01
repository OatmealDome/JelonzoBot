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
using Syroot.NintenTools.Byaml.Serialization;
using Nintendo.Blitz.Bcat.Versus;
using Syroot.BinaryData;
using Nintendo.Blitz.Bcat.Coop;
using Nintendo.Blitz.Bcat.Festival;
using JelonzoBot.Core.Config;

namespace JelonzoBot.Scheduler.Job
{
    public class BcatCheckerJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                foreach (KeyValuePair<RomType, BcatPair> bcatPairEntry in Program.BcatPairs)
                {
                    // Log that we're about to begin a check
                    await DiscordBot.LoggingChannel.SendMessageAsync("**[BCAT]** Beginning topic download for " + bcatPairEntry.Key.ToString());

                    // Download the latest Topic
                    Topic topic = await BcatApi.GetDataTopic(bcatPairEntry.Value.TitleId, bcatPairEntry.Value.Passphrase);

                    // Create the target folder name
                    string targetFolder = string.Format(Program.LOCAL_OLD_DATA_DIRECTORY, DateTime.Now.ToString(Program.FOLDER_DATE_TIME_FORMAT), bcatPairEntry.Key.ToString());

                    // Load the old Topic
                    Topic oldTopic = MessagePackSerializer.Deserialize<Topic>(File.ReadAllBytes(Program.LOCAL_LAST_TOPIC));

    #if DEBUG
                    if (!Configuration.LoadedConfiguration.IsProduction)
                    {
                        /*foreach (Bcat.Directory dir in oldTopic.Directories)
                        {
                            if (dir.Name == "line_news")
                            {
                                Data dbgData = dir.Data.FirstOrDefault();
                                if (dbgData == null)
                                {
                                    continue;
                                }
                                dir.Data.Remove(dbgData);
                                //dbgData.Digest = "deadbeef";
                            }
                        }*/
                    }
    #endif

                    // Get the differences
                    List<KeyValuePair<DifferenceType, string>> differences = BcatCheckerUtils.GetTopicChanges(oldTopic, topic);

                    // Check if there aren't any
                    if (differences.Count == 0)
                    {
                        // Nothing to do here
                        goto finished;
                    }

                    // Download all data
                    Dictionary<string, byte[]> data = await BcatCheckerUtils.DownloadAllData(topic, bcatPairEntry.Value.TitleId, bcatPairEntry.Value.Passphrase, targetFolder);

                    // Loop over every difference
                    foreach (KeyValuePair<DifferenceType, string> differencePair in differences)
                    {
                        // Log the difference
                        await DiscordBot.LoggingChannel.SendMessageAsync("**[BCAT]** diff: ``" + differencePair.Value + "`` (" + differencePair.Key.ToString() + ")");

                        // Get the FileType
                        FileType fileType = FileTypeExtensions.GetTypeFromFilePath(differencePair.Value);

                        // Create a ByamlSerializer instance
                        ByamlSerializer serializer = new ByamlSerializer(new ByamlSerializerSettings()
                        {
                            ByteOrder = ByteOrder.LittleEndian,
                            SupportPaths = false,
                            Version = ByamlVersion.Version1
                        });

                        // Create the difference handler parameters based off the FileType and difference type
                        object[] parameters;

                        if (differencePair.Key != DifferenceType.Removed)
                        {
                            // Get the raw file
                            byte[] rawFile = data[differencePair.Value];

                            // Deserialize the object from byaml if necessary
                            object deserializedObject;
                            using (MemoryStream memoryStream = new MemoryStream(rawFile))
                            {
                                switch (fileType)
                                {
                                    case FileType.VersusSetting:
                                        deserializedObject = serializer.Deserialize<VersusSetting>(memoryStream);
                                        break;
                                    case FileType.CoopSetting:
                                        deserializedObject = serializer.Deserialize<CoopSetting>(memoryStream);
                                        break;
                                    case FileType.FestivalByaml:
                                        deserializedObject = serializer.Deserialize<FestivalSetting>(memoryStream);
                                        break;
                                    default:
                                        deserializedObject = data[differencePair.Value];
                                        break;
                                }
                            }

                            if (differencePair.Key == DifferenceType.Added)
                            {
                                parameters = new object[] { bcatPairEntry.Key, data, deserializedObject, rawFile };
                            }
                            else // Changed
                            {
                                // Get the previous file
                                object previousFile;
                                switch (fileType)
                                {
                                    case FileType.VersusSetting:
                                        previousFile = FileCache.GetVersusSettingForRomType(bcatPairEntry.Key);
                                        break;
                                    case FileType.CoopSetting:
                                        previousFile = FileCache.GetCoopSetting();
                                        break;
                                    case FileType.FestivalByaml:
                                        previousFile = FileCache.GetLatestFestivalSettingForRomType(bcatPairEntry.Key);
                                        break;
                                    default:
                                        // Construct the previous file path
                                        string previousPath = Path.Combine((Configuration.LoadedConfiguration as JelonzoBotConfiguration).LastDownloadPaths[bcatPairEntry.Key], differencePair.Value.Replace('/', Path.DirectorySeparatorChar));

                                        // Load it
                                        previousFile = File.ReadAllBytes(previousPath);;
                                        break;
                                }

                                // Create the parameters
                                parameters = new object[] { bcatPairEntry.Key, data, previousFile, deserializedObject, rawFile };
                            }
                        }
                        else
                        {
                            parameters = new object[] { bcatPairEntry.Key, data };
                        }

                        // Call every difference handler
                        await BcatCheckerUtils.CallDifferenceHandlers((int)fileType, differencePair.Key, parameters);
                    }

                    // Write out the Topic
                    File.WriteAllBytes(Program.LOCAL_LAST_TOPIC, MessagePackSerializer.Serialize(topic));

finished:
                    await DiscordBot.LoggingChannel.SendMessageAsync("**[BCAT]** Check complete for " + bcatPairEntry.Key.ToString());
                }
            }
            catch (Exception exception)
            {
                // Notify the logging channel
                await DiscordUtil.HandleException(exception, $"in ``BcatCheckerJob``");
            }
        }

    }
}