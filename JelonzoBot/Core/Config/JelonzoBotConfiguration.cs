using System.Collections.Generic;
using BcatBotFramework.Core.Config;
using Nintendo.Blitz;

namespace JelonzoBot.Core.Config
{
    public class JelonzoBotConfiguration : Configuration
    {
        public Dictionary<RomType, string> LastDownloadPaths
        {
            get;
            set;
        }

        protected override void SetAppSpecificDefaults()
        {
            LastDownloadPaths = new Dictionary<RomType, string>();
        }

    }
}