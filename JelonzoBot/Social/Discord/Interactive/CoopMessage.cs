using System;
using System.Collections.Generic;
using System.Linq;
using BcatBotFramework.Internationalization;
using BcatBotFramework.Social.Discord;
using BcatBotFramework.Social.Discord.Interactive;
using Discord;
using JelonzoBot.Blitz.Internationalization;
using JelonzoBot.Core;
using Nintendo.Bcat;
using Nintendo.Blitz.Bcat.Coop;

namespace JelonzoBot.Social.Discord.Interactive
{
    public class CoopMessage : PagedInteractiveMessage
    {
        private List<CoopPhase> phases;

        private Language language;

        protected override int LastPage => phases.Count - 1;

        public CoopMessage(IUser user, Language lang) : base(user)
        {
            // Get the current CoopSetting
            CoopSetting coopSetting = FileCache.GetCoopSetting();

            // Get the current DateTime in UTC
            DateTime utcNow = DateTime.UtcNow;

            // Find the current and future phases
            phases = coopSetting.Phases.Where(x =>
            {
                return x.EndDateTime >= utcNow // is in future
                    || (x.StartDateTime <= utcNow && x.EndDateTime > utcNow); // is happening now
            }).ToList();

            // Set the Language
            language = lang;
        }

        public override MessageProperties CreateMessageProperties()
        {
            // Get the current phase
            CoopPhase phase = phases[this.CurrentPage];

            // Build the weapon string
            string weapons = string.Join('\n', phase.WeaponSets.Select(x => BlitzLocalizer.LocalizeWeapon(language, x)));

            // Build an Embed
            Embed embed = new EmbedBuilder()
                .WithTitle(Localizer.Localize("coop.title", language))
                .AddField(Localizer.Localize("coop.start_time", language), phase.StartDateTime <= DateTime.UtcNow ? Localizer.Localize("coop.start_time_now", language) : Localizer.LocalizeDateTime(phase.StartDateTime, language))
                .AddField(Localizer.Localize("coop.end_time", language), Localizer.LocalizeDateTime(phase.EndDateTime, language))
                .AddField(Localizer.Localize("coop.stage", language), BlitzLocalizer.LocalizeMap(language, phase.StageId))
                .AddField(Localizer.Localize("coop.weapons", language), weapons)
                .Build();

            // Return the Embed in a MessageProperties
            return new MessageProperties()
            {
                Embed = embed
            };
        }

    }
}