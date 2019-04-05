using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter
{
    [ByamlObject]
    public class RateRanges
    {
        [ByamlMember]
        public List<int> Festival
        {
            get;
            set;
        }

        [ByamlMember]
        public List<int> FestivalTeam
        {
            get;
            set;
        }

        [ByamlMember]
        public List<int> Regular
        {
            get;
            set;
        }

        [ByamlMember("RDAdjustment")]
        public RDAdjustments RDAdjustments
        {
            get;
            set;
        }

    }
}