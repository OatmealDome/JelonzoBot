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

        public override void SetDefaults()
        {
            RemoteServer = null;
            LatestFestivalManifestPath = "/home/oatmealdome/www/latest_festival.json";
        }

    }
}