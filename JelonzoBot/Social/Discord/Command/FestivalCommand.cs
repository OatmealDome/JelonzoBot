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

            // Localize the RomType
            string localizedRomType = Localizer.Localize($"romtype.{romType.ToString().ToLower()}", language);

            // Use the special title if necessary
            string title;
            if (festivalSetting.SpecialType != null)
            {
                // Localize the special type
                string localizedSpecialType = Localizer.Localize($"festival.special_type.{festivalSetting.SpecialType.ToLower()}", language);

                // Localize the title
                title = string.Format(Localizer.Localize("festival.title_special", language), localizedRomType, localizedSpecialType);
            }
            else
            {
                // Use the standard title
                title = string.Format(Localizer.Localize("festival.title", language), localizedRomType);
            }

            // Begin building an embed
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle(title)
                .AddField(Localizer.Localize("festival.team_alpha", language), $"**{festivalSetting.Teams[0].Name[language]}**", true)
                .AddField(Localizer.Localize("festival.team_bravo", language), $"**{festivalSetting.Teams[1].Name[language]}**", true)
                .AddField(Localizer.Localize("festival.rule", language), BlitzLocalizer.LocalizeRule(festivalSetting.VersusRule, language), true)
                .AddField(Localizer.Localize("festival.special_stage", language), BlitzLocalizer.LocalizeStage(festivalSetting.SpecialStage, language), true)
                .AddField(Localizer.Localize("festival.announcement_time", language), Localizer.LocalizeDateTime(festivalSetting.Times.Announcement, language))
                .AddField(Localizer.Localize("festival.period", language), string.Format(Localizer.Localize("festival.period_format", language), Localizer.LocalizeDateTime(festivalSetting.Times.Start, language), Localizer.LocalizeDateTime(festivalSetting.Times.End, language)))
                .AddField(Localizer.Localize("festival.results_time", language), Localizer.LocalizeDateTime(festivalSetting.Times.Result, language))
                .WithImageUrl($"https://cdn.oatmealdome.me/splatoon/festival/{romType.ToString()}/{festivalSetting.FestivalId}/panel.png")
                .WithColor(new Color(drawingColor.R, drawingColor.G, drawingColor.B));

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

    }
}