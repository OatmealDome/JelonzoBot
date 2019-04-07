using System;
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
using Nintendo.Blitz;
using JelonzoBot.Core;

namespace JelonzoBot
{
    class Program
    {
        // Local Directory
        private static string LOCAL_DIRECTORY = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string LOCAL_LAST_TOPIC = Path.Combine(LOCAL_DIRECTORY, "last_topic_{0}");
        public static string LOCAL_OLD_DATA_DIRECTORY = Path.Combine(LOCAL_DIRECTORY, "DownloadedData", "{0}-{1}");

        public static string FOLDER_DATE_TIME_FORMAT = "MMddyy-HHmmss";

        public static Dictionary<RomType, BcatPair> BcatPairs = new Dictionary<RomType, BcatPair>()
        {
            { RomType.NorthAmerica, new BcatPair("01003bc0000a0000","de13a4a0894bba7308ccc5c2fd5cd6f8e4ebded4dbd1131b6608ddfc7f1f8231") },
            { RomType.Europe, new BcatPair("0100f8f0000a2000", "09f9013710ff0e58c900a6c8e19381f42b7a94d8f8d5ff9c3af9f153a397556f") },
            { RomType.Japan, new BcatPair("01003c700009c000", "292bad61c9f52d7fd505dc8d7c4076afb23d7c3b49d36b042f490bee7ec6f5e0") }
        };

    }
}
