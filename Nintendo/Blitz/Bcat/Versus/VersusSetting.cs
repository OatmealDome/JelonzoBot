using System.Collections.Generic;
using Nintendo.Blitz.Bcat.Versus.Parameter;
using Nintendo.Blitz.Bcat.Versus.Schedule;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus
{
    [ByamlObject]
    public class VersusSetting
    {
        [ByamlMember]
        public BgmSelect BgmSelect
        {
            get;
            set;
        }

        [ByamlMember("MapFirstAppear")]
        public List<MapUnlock> MapUnlocks
        {
            get;
            set;
        }

        [ByamlMember("Parameter")]
        public VersusParameters Parameters
        {
            get;
            set;
        }

        [ByamlMember]
        public List<Phase> Phases
        {
            get;
            set;
        }

        [ByamlMember]
        public List<PhasesSeed> PhasesSeeds
        {
            get;
            set;
        }

        [ByamlMember("RuleFirstAppear")]
        public List<RuleUnlock> RuleUnlocks
        {
            get;
            set;
        }

        [ByamlMember("WeaponUnlock")]
        public List<WeaponUnlock> WeaponUnlocks
        {
            get;
            set;
        }
        
    }
}