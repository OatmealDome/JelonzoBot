using System;
using System.Collections.Generic;
using Nintendo.Blitz.Bcat;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Coop
{
    [ByamlObject]
    public class CoopPhase : IByamlSerializable
    {
        public DateTime StartDateTime
        {
            get;
            set;
        }

        public DateTime EndDateTime
        {
            get;
            set;
        }

        [ByamlMember("StageID")]
        public int StageId
        {
            get;
            set;
        }

        [ByamlMember("RareWeaponID")]
        public int RareWeaponId
        {
            get;
            set;
        }

        [ByamlMember]
        public List<int> WeaponSets
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            StartDateTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["StartDateTime"]);
            StartDateTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["EndDateTime"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}