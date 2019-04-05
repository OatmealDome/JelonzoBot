using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Schedule
{
    [ByamlObject]
    public class Phase : IByamlSerializable
    {
        public DateTime DateTime
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public PhaseSetup Regular
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public PhaseSetup Gachi
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public PhaseSetup League
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            DateTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["DateTime"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}