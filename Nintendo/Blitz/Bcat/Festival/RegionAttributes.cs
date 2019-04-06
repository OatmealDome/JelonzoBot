using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Festival
{
    [ByamlObject]
    public class RegionAttributes
    {
        [ByamlMember]
        public int US
        {
            get;
            set;
        }

        [ByamlMember]
        public int EU
        {
            get;
            set;
        }

        [ByamlMember]
        public int JP
        {
            get;
            set;
        }

    }
}