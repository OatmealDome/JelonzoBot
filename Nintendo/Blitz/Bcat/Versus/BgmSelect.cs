using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus
{
    [ByamlObject]
    public class BgmSelect : IByamlSerializable
    {
        public DateTime Begin
        {
            get;
            set;
        }

        public DateTime End
        {
            get;
            set;
        }

        public float PreferRate
        {
            get;
            set;
        }

        [ByamlMember("PreferBgmIds")]
        public List<int> PreferredIds
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            Begin = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["BeginTime"]);
            End = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["EndTime"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}