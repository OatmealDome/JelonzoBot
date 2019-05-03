using System.Threading.Tasks;
using BcatBotFramework.Internationalization;
using BcatBotFramework.Social.Discord;
using Discord;
using Discord.Commands;
using JelonzoBot.Blitz.Internationalization;
using JelonzoBot.Core;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Festival;

namespace JelonzoBot.Social.Discord.Command
{
    public class FestivalCommand : ModuleBase<SocketCommandContext>
    {
        [Command("Festival"), Summary("Shows Splatfest data")]
        public async Task Execute(string romTypeStr = null, string languageStr = null)
        {
            // Parse the specified RomType
            RomType romType;
            if (romTypeStr != null)
            {
                // Get the RomType
                romType = (RomType)EnumUtil.GetEnumValueFromString(typeof(RomType), romTypeStr);
            }
            else
            {
                // Default to North America
                romType = RomType.NorthAmerica;
            }

            // Get the FestivalSetting for the specified RomType
            FestivalSetting festivalSetting = FileCache.GetLatestFestivalSettingForRomType(romType);

            // Get the default language
            Language language = DiscordUtil.GetDefaultLanguage(Context.Guild, languageStr);

            // Get the Color for the neutral team
            System.Drawing.Color drawingColor = festivalSetting.Teams[2].GetColor4fAsColor();

            // Begin building an embed
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle(Localizer.Localize("festival.title", language))
                .AddField(Localizer.Localize("festival.team_alpha", language), $"**{festivalSetting.Teams[0].Name[language]}**", true)
                .AddField(Localizer.Localize("festival.team_bravo", language), $"**{festivalSetting.Teams[1].Name[language]}**", true)
                .AddField(Localizer.Localize("festival.rule", language), festivalSetting.VersusRule.ToString(), true)
                .AddField(Localizer.Localize("festival.special_stage", language), BlitzLocalizer.LocalizeStage(festivalSetting.SpecialStage, language), true);

            // Add the special type field if necessary
            if (festivalSetting.SpecialType != null)
            {
                embedBuilder.AddField(Localizer.Localize("festival.special_type", language), festivalSetting.SpecialType);
            }

             // Continue adding fields
            embedBuilder.AddField(Localizer.Localize("festival.announcement_time", language), Localizer.LocalizeDateTime(festivalSetting.Times.Announcement, language))
                .AddField(Localizer.Localize("festival.period", language), string.Format(Localizer.Localize("festival.period_format", language), Localizer.LocalizeDateTime(festivalSetting.Times.Start, language), Localizer.LocalizeDateTime(festivalSetting.Times.End, language)))
                .AddField(Localizer.Localize("festival.results_time", language), Localizer.LocalizeDateTime(festivalSetting.Times.Result, language))
                .WithColor(new Color(drawingColor.R, drawingColor.G, drawingColor.B));

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

    }
}