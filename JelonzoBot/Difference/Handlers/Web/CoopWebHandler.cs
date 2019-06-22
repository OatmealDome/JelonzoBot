using System.Collections.Generic;
using System.Threading.Tasks;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Difference;
using JelonzoBot.Blitz;
using JelonzoBot.Core.Config;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Coop;

namespace JelonzoBot.Difference.Handlers.Web
{
    public class CoopWebHandler
    {
        [JelonzoBotDifferenceHandler(FileType.CoopSetting, DifferenceType.Changed, 1)]
        public static Task HandleCoop(RomType romType, Dictionary<string, byte[]> data, CoopSetting previousSetting, CoopSetting newSetting, byte[] rawFile)
        {
            // Only do this once
            if (romType != RomType.NorthAmerica)
            {
                return Task.FromResult(0);
            }

            lock (WebFileHandler.Lock)
            {
                // Get the WebConfig
                JelonzoBotWebConfig webConfig = ((JelonzoBotConfiguration)Configuration.LoadedConfiguration).WebConfig;

                // Connect to the remote server if needed
                WebFileHandler.Connect(webConfig);

                // Deserialize the CoopSetting dynamically
                dynamic settingDynamic = ByamlLoader.GetByamlDynamic(rawFile);

                // Upload to the server
                WebFileHandler.WriteSerializedJson(webConfig.CoopSettingPath, settingDynamic);

                // Disconnect
                WebFileHandler.Disconnect();
            }

            return Task.FromResult(0);
        }

    }
}