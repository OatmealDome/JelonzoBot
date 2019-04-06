using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Festival
{
    [ByamlObject]
    public class Weighting
    {
        [ByamlMember("count")]
        public int Count
        {
            get;
            set;
        }

        [ByamlMember("point")]
        public int Point
        {
            get;
            set;
        }

        [ByamlMember("yellow")]
        public int Yellow
        {
            get;
            set;
        }

    }
}