using System.Collections.Generic;
using Nintendo.Blitz.News.Command;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.News.Romfs
{
    [ByamlObject]
    public class ScriptWrapper : IByamlSerializable
    {
        [ByamlMember]
        public string NewsType
        {
            get;
            set;
        }

        public List<ScriptCommand> Commands
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            //System.Console.WriteLine(dictionary["Commands"]);
            Commands = ScriptParser.ParseCommandList((List<object>)dictionary["Commands"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new System.NotImplementedException();
        }

    }
}