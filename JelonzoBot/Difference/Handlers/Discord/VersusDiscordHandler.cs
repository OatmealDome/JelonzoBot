using System.Collections.Generic;
using System.Threading.Tasks;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Difference;
using BcatBotFramework.Social.Discord;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Versus;

namespace JelonzoBot.Difference.Handlers.Discord
{
    public class VersusDiscordHandler
    {
        [JelonzoBotDifferenceHandler(FileType.VersusSetting, DifferenceType.Changed, 100)]
        public static async Task HandleVersus(RomType romType, Dictionary<string, byte[]> data, VersusSetting previousSetting, VersusSetting newSetting, byte[] rawFile)
        {
            await DiscordBot.LoggingChannel.SendMessageAsync($"**[VS] <@{Configuration.LoadedConfiguration.DiscordConfig.AdministratorIds[0]}> Settings for {romType.ToString()} updated.");
        }

    }
}