using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Difference;
using BcatBotFramework.Internationalization;
using BcatBotFramework.Social.Twitter;
using JelonzoBot.Core;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Festival;
using S3;

namespace JelonzoBot.Difference.Handlers.Twitter
{
    public class FestivalTwitterHandler
    {
        [JelonzoBotDifferenceHandler(FileType.FestivalByaml, DifferenceType.Changed, 51)]
        public static Task HandleFestival(RomType romType, Dictionary<string, byte[]> data, FestivalSetting previousFestival, FestivalSetting newFestival, byte[] rawFile)
        {
            // Don't do anything
            if (previousFestival.FestivalId == newFestival.FestivalId)
            {
                //return Task.FromResult(0);
            }

            // Get the language based off the RomType
            Language language;
            switch (romType)
            {
                case RomType.NorthAmerica:
                    language = Language.EnglishUS;
                    break;
                case RomType.Europe:
                    language = Language.EnglishUK;
                    break;
                case RomType.Japan:
                    language = Language.Japanese;
                    break;
                default:
                    throw new Exception("Unsupported RomType (A)");
            }

            // Create the title
            string localizedRomType = Localizer.Localize(string.Format("romtype.{0}", romType.ToString().ToLower()), language);
            string title = string.Format(Localizer.Localize("festival.twitter.title", language), localizedRomType);

            // Create the description
            string localizedDescription = Localizer.Localize("festival.twitter.description", language);
            string startTime = Localizer.LocalizeDateTime(newFestival.Times.Start, language);
            string endTime = Localizer.LocalizeDateTime(newFestival.Times.End, language);
            localizedDescription = string.Format(localizedDescription, newFestival.Teams[0].ShortName[language], newFestival.Teams[1].ShortName[language], startTime, endTime);

            // Get the region code based off the RomType
            string regionCode;
            switch (romType)
            {
                case RomType.NorthAmerica:
                    regionCode = "na";
                    break;
                case RomType.Europe:
                    regionCode = "eu";
                    break;
                case RomType.Japan:
                    regionCode = "jp";
                    break;
                default:
                    throw new Exception("Unsupported RomType (B)");
            }

            // Create the URL
            string url = string.Format(Localizer.Localize("festival.twitter.url", language), regionCode);

            // Get the target Twitter account
            string targetAccount;
            if (Configuration.LoadedConfiguration.IsProduction)
            {
                if (romType == RomType.NorthAmerica || romType == RomType.Europe)
                {
                    targetAccount = "JelonzoBot";
                }
                else if (romType == RomType.Japan)
                {
                    targetAccount = "JelonzoBotJP";
                }
                else
                {
                    throw new Exception("Unsupported RomType (C)");
                }
            }
            else
            {
                targetAccount = "JelonzoTest";
            }

            // Get the image
            string imagePath = string.Format(FileCache.FESTIVAL_PANEL_PATH, romType.ToString(), newFestival.FestivalId);
            byte[] image = File.ReadAllBytes(imagePath);
            
            // Tweet
            TwitterManager.GetAccount(targetAccount).Tweet(title, localizedDescription, url, image);

            return Task.FromResult(0);
        }

    }
}