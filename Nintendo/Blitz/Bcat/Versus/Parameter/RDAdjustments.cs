using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter
{
    [ByamlObject]
    public class RDAdjustments
    {
        [ByamlMember]
        public float Festival
        {
            get;
            set;
        }

        [ByamlMember]
        public float FestivalTeam
        {
            get;
            set;
        }

        [ByamlMember]
        public float Regular
        {
            get;
            set;
        }
        
    }
}