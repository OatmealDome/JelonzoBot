using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat
{
    [ByamlObject]
    public class GlickoConstants : IByamlSerializable
    {
        public int HighRate1
        {
            get;
            set;
        }
        
        public int HighRate2
        {
            get;
            set;
        }

        public int LowRate
        {
            get;
            set;
        }

        public float ShrinkRatio
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> sourceDict)
        {
            HighRate1 = (int)sourceDict["HighRate1"];
            HighRate2 = (int)sourceDict["HighRate2"];
            LowRate = (int)sourceDict["LowRate"];
            ShrinkRatio = (float)sourceDict["ShrinkRatio"];
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}