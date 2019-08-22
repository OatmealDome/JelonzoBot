using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Scheduler;
using BcatBotFramework.Scheduler.Job;
using BcatBotFramework.Social.Discord;
using JelonzoBot.Blitz;
using JelonzoBot.Blitz.Internationalization;
using JelonzoBot.Core;
using JelonzoBot.Core.Config;
using Quartz;

namespace JelonzoBot.Scheduler.Job
{
    public class JelonzoBotBootHousekeepingJob : BootHousekeepingJob
    {
        protected override async Task RunAppSpecificBootTasks()
        {
            // Initialize the FileCache if first run is complete
            if (Configuration.LoadedConfiguration.FirstRunCompleted)
            {
                FileCache.Initialize();
            }

            // Initialize the RomResourceLoader
            RomResourceLoader.Initialize();

            // Initialize the BlitzLocalizer
            BlitzLocalizer.Initialize();

            // Load GameConfigSetting
            XDocument gameConfig = XDocument.Load(RomResourceLoader.GetRomFile("/System/GameConfigSetting.xml"));

            // Get the application version
            int appVersion = int.Parse(gameConfig.Root
                .Elements("category").Where(e => e.Attribute("name").Value == "Root").First()
                .Elements("category").Where(e => e.Attribute("name").Value == "Project").First()
                .Elements("category").Where(e => e.Attribute("name").Value == "Version").First()
                .Elements("parameter").Where(e => e.Attribute("name").Value == "AppVersion").First()
                .Attribute("defaultValue").Value);

            // Output the ROM version
            await DiscordBot.LoggingChannel.SendMessageAsync($"**[JelonzoBotBootHousekepingJob]** ROM version {appVersion} was loaded by RomResourceLoader");

            // Get the ROM config
            RomConfig romConfig = (Configuration.LoadedConfiguration as JelonzoBotConfiguration).RomConfig;

            // Check if this version is new compared to the last boot
            if (romConfig.LastRomVersion < appVersion)
            {
                // Create a JobDataMap to hold the version
                JobDataMap dataMap = new JobDataMap();
                dataMap.Add("version", appVersion);

                // Upload necessary ROM data after everything is initalized
                await QuartzScheduler.ScheduleJob<RomDataUploadJob>("Normal", DateTime.Now.AddSeconds(5), dataMap);
            }
        }

        protected override async Task SchedulePostBootJobs()
        {
            if (!Configuration.LoadedConfiguration.FirstRunCompleted)
            {
                await QuartzScheduler.ScheduleJob<BcatFirstRunJob>("Normal");
            }
            else if (Configuration.LoadedConfiguration.IsProduction) 
            {
                await QuartzScheduler.ScheduleJob<BcatCheckerJob>("Normal", Configuration.LoadedConfiguration.JobSchedules["Bcat"]);
            }
        }
        
    }
}