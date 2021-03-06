using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Schedule
{
    [ByamlObject]
    public class PhaseSetup : IByamlSerializable
    {
        [ByamlMember("MapID0")]
        public int MapOne
        {
            get;
            set;
        }

        [ByamlMember("MapID1")]
        public int MapTwo
        {
            get;
            set;
        }

        public VersusRule Rule
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            Rule = (VersusRule)EnumUtil.GetEnumValueFromString(typeof(VersusRule), (string)dictionary["Rule"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}