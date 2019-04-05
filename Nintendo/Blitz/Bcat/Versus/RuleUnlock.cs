using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus
{
    [ByamlObject]
    public class RuleUnlock : IByamlSerializable
    {
        public DateTime UnlockTime
        {
            get;
            set;
        }

        public VersusRule VersusRule
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            // Deserialize the time
            UnlockTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["DateTime"]);

            // Get the enum value for the VersusRule
            VersusRule = (VersusRule)BlitzBcatDeserializationUtil.GetEnumValueFromString(typeof(VersusRule), (string)dictionary["VersusRule"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}