using Syroot.NintenTools.Byaml.Serialization;

namespace NIntendo.Blitz.Bcat.Festival
{
    [ByamlObject]
    public class WeightMatchRequirements
    {
        [ByamlMember("WeightTenMatch")]
        public Weighting Ten
        {
            get;
            set;
        }

        [ByamlMember("WeightHundredMatch")]
        public Weighting Hundred
        {
            get;
            set;
        }
        
    }
}