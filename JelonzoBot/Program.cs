﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Nintendo.Bcat;
using Nintendo.DAuth;
using BcatBotFramework.Difference;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Core.Config.Discord;
using BcatBotFramework.Core.Config.Scheduler;
using S3;
using BcatBotFramework.Scheduler;
using BcatBotFramework.Social.Discord;
using BcatBotFramework.Core.Config.Twitter;
using BcatBotFramework.Social.Twitter;
using BcatBotFramework.Internationalization;
using JelonzoBot.Core.Config;
using JelonzoBot.Scheduler.Job;

namespace JelonzoBot
{
    class Program
    {
        // Local Directory
        private static string LOCAL_DIRECTORY = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string LOCAL_CONFIGURATION = Path.Combine(LOCAL_DIRECTORY, "config.json");
        public static string LOCAL_LAST_TOPIC = Path.Combine(LOCAL_DIRECTORY, "last_topic");
        public static string LOCAL_AUTOMATIC_RESTART_DISABLE_FLAG = Path.Combine(LOCAL_DIRECTORY, "no_automatic_restart");
        public static string LOCAL_OLD_DATA_DIRECTORY = Path.Combine(LOCAL_DIRECTORY, "DownloadedData", "{0}");
        public static string LOCAL_CONTAINER_CACHE_DIRECTORY = Path.Combine(LOCAL_DIRECTORY, "ContainerCache");
        public static string LOCAL_COMMON_CACHE_DIRECTORY = Path.Combine(LOCAL_DIRECTORY, "CommonCache");
        public static string LOCAL_EXCEPTION_LOGS_DIRECTORY = Path.Combine(LOCAL_DIRECTORY, "ExceptionLogs");

        public static string FOLDER_DATE_TIME_FORMAT = "yyyy-MM-dd-HH-mm";
        
        static async Task Main(string[] args)
        {
            // Wait for the debugger to attach if requested
            if (args.Length > 0 && args[0].ToLower() == "--waitfordebugger")
            {
                Console.WriteLine("Waiting for debugger...");

                while (!Debugger.IsAttached)
                {
                    await Task.Delay(1000);
                }

                Console.WriteLine("Debugger attached!");
            }

            // Declare variable to hold the configuration
            JelonzoBotConfiguration configuration;

            // Check if the config file exists
            if (!File.Exists(LOCAL_CONFIGURATION))
            {
                // Create a new dummy Configuration
                configuration = new JelonzoBotConfiguration();

                // Set defaults
                configuration.CdnConfig = new NintendoCdnConfig();
                configuration.CdnConfig.SetToDefaults();
                configuration.DiscordConfig = new DiscordConfig();
                configuration.DiscordConfig.Token = "cafebabe";
                configuration.DiscordConfig.ClientId = 0;
                configuration.DiscordConfig.Permissions = 0;
                configuration.DiscordConfig.AdministratorIds = new List<ulong>()
                {
                    112966101368901632, // OatmealDome
                    180994059542855681 // Simonx22
                };
                configuration.DiscordConfig.LoggingTargetChannel = new GuildSettings();
                configuration.DiscordConfig.AlternatorRate = 30;
                configuration.DiscordConfig.CommandStatistics = new ConcurrentDictionary<string, ulong>();
                configuration.DiscordConfig.GuildSettings = new List<GuildSettings>();
                configuration.S3Config = new S3Config();
                configuration.S3Config.ServiceUrl = "https://s3.example.com";
                configuration.S3Config.BucketName = "bucket";
                configuration.S3Config.AccessKey = "cafebabe";
                configuration.S3Config.AccessKeySecret = "deadbeef";
                configuration.JobSchedules = new Dictionary<string, JobSchedule>();
                configuration.TwitterConfig = new TwitterConfig();
                configuration.TwitterConfig.TwitterCredentials = new Dictionary<string, CachedTwitterCredentials>();
                configuration.TwitterConfig.CharacterCounterBinary = "/home/oatmealdome/characterCounter";
                configuration.TwitterConfig.IsActivated = true;
                configuration.FirstRunCompleted = false;

                // Write out the default config
                configuration.Write();

                Console.WriteLine("Wrote default configuration to " + LOCAL_CONFIGURATION);

                return;
            }

            // Create the common cache folder
            System.IO.Directory.CreateDirectory(Program.LOCAL_COMMON_CACHE_DIRECTORY);

            // Create the Exception logs directory
            System.IO.Directory.CreateDirectory(Program.LOCAL_EXCEPTION_LOGS_DIRECTORY);

            // Load the Configuration
            Configuration.Load<JelonzoBotConfiguration>(LOCAL_CONFIGURATION);

            // Initialize the Localizer
            Localizer.Initialize();

            // Initialize the HandlerMapper
            HandlerMapper.Initialize();

            // Initialize DAuth
            DAuthApi.Initialize();

            // Initialize BCAT
            BcatApi.Initialize();

            // Initialize S3
            S3Api.Initialize();

            // Initialize Twitter
            TwitterManager.Initialize();

            // Initialize the DiscordBot
            await DiscordBot.Initialize();

            // Initialize the Scheduler
            await QuartzScheduler.Initialize();

            // Wait for the bot to fully initialize
            while (!DiscordBot.IsReady)
            {
                await Task.Delay(1000);
            }

            // Print out to the logging channel that we're initialized
            await DiscordBot.LoggingChannel.SendMessageAsync("\\*\\*\\* **Initialized**");

            // Schedule the BootHousekeepingJob
            await QuartzScheduler.ScheduleJob<JelonzoBotBootHousekeepingJob>("Immediate");
            
            await Task.Delay(-1);

        }

    }
}
