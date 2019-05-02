using System.Collections.Generic;
using BcatBotFramework.Core.Config;
using Newtonsoft.Json;
using Nintendo.Blitz;

namespace JelonzoBot.Core.Config
{
    public class JelonzoBotConfiguration : Configuration
    {
        [JsonProperty("Web")]
        public JelonzoBotWebConfig WebConfig
        {
            get;
            set;
        }

        public Dictionary<RomType, string> LastDownloadPaths
        {
            get;
            set;
        }

        protected override void SetAppSpecificDefaults()
        {
            WebConfig = new JelonzoBotWebConfig();
            WebConfig.SetDefaults();
            LastDownloadPaths = new Dictionary<RomType, string>();
        }

    }
}