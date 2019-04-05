using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus
{
    [ByamlObject]
    public class MapUnlock : IByamlSerializable
    {
        public DateTime UnlockTime
        {
            get;
            set;
        }

        [ByamlMember("MapID")]
        public int MapId
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            UnlockTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["DateTime"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}