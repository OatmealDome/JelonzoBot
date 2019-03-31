using System;
using System.Collections.Generic;
using Nintendo.Blitz.Gear;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Coop
{
    [ByamlObject]
    public class RewardGear : IByamlSerializable
    {
        public DateTime StartDateTime
        {
            get;
            set;
        }

        [ByamlMember("GearID")]
        public int GearId
        {
            get;
            set;
        }

        [ByamlMember("GearKind")]
        public GearType GearType
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            StartDateTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["DateTime"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}