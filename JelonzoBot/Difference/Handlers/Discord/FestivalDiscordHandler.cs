using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BcatBotFramework.Difference;
using BcatBotFramework.Internationalization;
using BcatBotFramework.Internationalization.Discord;
using BcatBotFramework.Social.Discord;
using Discord;
using JelonzoBot.Blitz.Internationalization;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Festival;

namespace JelonzoBot.Difference.Handlers.Discord
{
    public class FestivalDiscordHandler
    {
        [JelonzoBotDifferenceHandler(FileType.FestivalByaml, DifferenceType.Changed, 50)]
        public static async Task HandleFestival(RomType romType, Dictionary<string, byte[]> data, FestivalSetting previousFestival, FestivalSetting newFestival, byte[] rawFile)
        {
            // Don't do anything
            if (previousFestival.FestivalId == newFestival.FestivalId)
            {
                return;
            }

            // Get the Color for the neutral team
            System.Drawing.Color drawingColor = newFestival.Teams[2].GetColor4fAsColor();

            // Localize the times
            Dictionary<Language, string> startTime = Localizer.LocalizeDateTimeToAllLanguages(newFestival.Times.Start);
            Dictionary<Language, string> endTime = Localizer.LocalizeDateTimeToAllLanguages(newFestival.Times.End);

            // Create the format parameters
            Dictionary<Language, object[]> formatParams = new Dictionary<Language, object[]>();
            foreach (Language language in BlitzUtil.SupportedLanguages)
            {
                formatParams[language] = new object[] { startTime[language], endTime[language] };
            }

            // Format the final string
            Dictionary<Language, string> periodStr = Localizer.LocalizeToAllLanguagesWithFormat("festival.period_format", BlitzUtil.SupportedLanguages, formatParams);

            Dictionary<Language, string> AddMissingLocalizations(Dictionary<Language, string> originalDict)
            {
                // Add any missing strings for team alpha
                Dictionary<Language, string> names = new Dictionary<Language, string>();
                foreach (Language language in BlitzUtil.SupportedLanguages)
                {
                    // Try getting the name from the original dictionary
                    if (originalDict.TryGetValue(language, out string name))
                    {
                        names.Add(language, $"**{name}**");
                    }
                    else
                    {
                        // Use the RomType to get the name
                        switch (romType)
                        {
                            case RomType.NorthAmerica:
                                names.Add(language, $"**{originalDict[Language.EnglishUS]}**");
                                break;
                            case RomType.Europe:
                                names.Add(language, $"**{originalDict[Language.EnglishUK]}**");
                                break;
                            case RomType.Japan:
                                names.Add(language, $"**{originalDict[Language.Japanese]}**");
                                break;
                            default:
                                throw new Exception("Unsupported RomType");
                        }
                    }
                }

                return names;
            }

            // Add any missing strings for the teams
            Dictionary<Language, string> alphaNames = AddMissingLocalizations(newFestival.Teams[0].Name);
            Dictionary<Language, string> bravoNames = AddMissingLocalizations(newFestival.Teams[1].Name);

            // Localize the RomType
            Dictionary<Language, string> localizedRomType = Localizer.LocalizeToAllLanguages($"romtype.{romType.ToString().ToLower()}");

            // Use the special title if necessary
            Dictionary<Language, string> localizedTitle;
            if (newFestival.SpecialType != null)
            {
                // Localize the special type
                Dictionary<Language, string> localizedSpecialTypes = Localizer.LocalizeToAllLanguages($"festival.special_type.{newFestival.SpecialType.ToLower()}");

                // Create the format parameters
                formatParams = new Dictionary<Language, object[]>();
                foreach (Language language in BlitzUtil.SupportedLanguages)
                {
                    formatParams[language] = new object[] { localizedRomType[language], localizedSpecialTypes[language] };
                }

                // Localize the title
                localizedTitle = Localizer.LocalizeToAllLanguagesWithFormat("festival.title_special", BlitzUtil.SupportedLanguages, formatParams);
            }
            else
            {
                // Create the format parameters
                formatParams = new Dictionary<Language, object[]>();
                foreach (Language language in BlitzUtil.SupportedLanguages)
                {
                    formatParams[language] = new object[] { localizedRomType[language] };
                }

                // Localize the title
                localizedTitle = Localizer.LocalizeToAllLanguagesWithFormat("festival.title", BlitzUtil.SupportedLanguages, formatParams);
            }

            // Start building the embed
            LocalizedEmbedBuilder embedBuilder = new LocalizedEmbedBuilder(BlitzUtil.SupportedLanguages)
                .WithTitle(localizedTitle)
                .AddField("festival.team_alpha", alphaNames, true)
                .AddField("festival.team_bravo", bravoNames, true)
                .AddField("festival.rule", BlitzLocalizer.LocalizeRuleToAllLanguages(newFestival.VersusRule), true)
                .AddField("festival.special_stage", BlitzLocalizer.LocalizeStageToAllLanguages(newFestival.SpecialStage), true)
                .AddField("festival.announcement_time", Localizer.LocalizeDateTimeToAllLanguages(newFestival.Times.Announcement))
                .AddField("festival.period", periodStr)
                .AddField("festival.results_time", Localizer.LocalizeDateTimeToAllLanguages(newFestival.Times.Result))
                .WithImageUrl($"https://cdn.oatmealdome.me/splatoon/festival/{romType.ToString()}/{newFestival.FestivalId}/panel.png")
                .WithColor(new Color(drawingColor.R, drawingColor.G, drawingColor.B));

            // Send the notification
            await DiscordBot.SendNotificationAsync("**[Splatfest]**", embedBuilder.Build());
        }

    }
}