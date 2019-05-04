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

        public override void SetDefaults()
        {
            RemoteServer = null;
            LatestFestivalManifestPath = "/home/oatmealdome/www/latest_festival.json";
            FestivalSettingPath = "/home/oatmealdome/www/festival_{0}.json";
        }

    }
}