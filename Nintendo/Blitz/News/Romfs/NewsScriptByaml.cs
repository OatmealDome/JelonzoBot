using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.News.Romfs
{
    [ByamlObject]
    public class NewsScriptByaml
    {
        [ByamlMember("News")]
        public List<ScriptWrapper> Scripts
        {
            get;
            set;
        }

    }
}