using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Difference;
using JelonzoBot.Blitz;
using JelonzoBot.Core.Config;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Versus;

namespace JelonzoBot.Difference.Handlers.Web
{
    public class VersusWebHandler
    {
        [JelonzoBotDifferenceHandler(FileType.VersusSetting, DifferenceType.Changed, 1)]
        public static Task HandleVersus(RomType romType, Dictionary<string, byte[]> data, VersusSetting previousSetting, VersusSetting newSetting, byte[] rawFile)
        {
            lock (WebFileHandler.Lock)
            {
                // Get the WebConfig
                JelonzoBotWebConfig webConfig = ((JelonzoBotConfiguration)Configuration.LoadedConfiguration).WebConfig;

                // Connect to the remote server if needed
                WebFileHandler.Connect(webConfig);

                // Deserialize the VersusSetting dynamically
                dynamic settingDynamic = ByamlLoader.GetByamlDynamic(rawFile);

                // Get the FestivalSetting JSON path
                string path = webConfig.VersusSettingPath;
                switch (romType)
                {
                    case RomType.NorthAmerica:
                        path = string.Format(path, "na");
                        break;
                    case RomType.Europe:
                        path = string.Format(path, "eu");
                        break;
                    case RomType.Japan:
                        path = string.Format(path, "jp");
                        break;
                    default:
                        throw new Exception("Invalid RomType");
                }

                // Upload to the server
                WebFileHandler.WriteSerializedJson(path, settingDynamic);

                // Disconnect
                WebFileHandler.Disconnect();
            }

            return Task.FromResult(0);
        }

    }
}