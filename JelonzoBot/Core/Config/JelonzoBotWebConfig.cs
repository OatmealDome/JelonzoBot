using BcatBotFramework.Core.Config;

namespace JelonzoBot.Core.Config
{
    public class JelonzoBotWebConfig : WebConfig
    {
        public string LatestFestivalManifestPath
        {
            get;
            set;
        }

        public string FestivalSettingPath
        {
            get;
            set;
        }

        public string CoopSettingPath
        {
            get;
            set;
        }

        public string VersusSettingPath
        {
            get;
            set;
        }

        public override void SetDefaults()
        {
            RemoteServer = null;
            LatestFestivalManifestPath = "/home/oatmealdome/www/latest_festival.json";
            FestivalSettingPath = "/home/oatmealdome/www/festival_{0}.json";
            VersusSettingPath = "/home/oatmealdome/www/versus_{0}.json";
            CoopSettingPath = "/home/oatmealdome/www/coop.json";
        }

    }
}