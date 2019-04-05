using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter.Udemae
{
    [ByamlObject]
    public class UdemaePointRetention : IByamlSerializable
    {
        [ByamlMember]
        public float Down
        {
            get;
            set;
        }

        [ByamlMember]
        public float Keep
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            Down = (float)dictionary["Down"];
            Keep = (float)dictionary["Keep"];
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}