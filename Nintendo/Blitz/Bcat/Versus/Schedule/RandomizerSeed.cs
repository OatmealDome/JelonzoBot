using System;
using System.Collections.Generic;
using System.Linq;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Schedule
{
    [ByamlObject]
    public class RandomizerSeed : IByamlSerializable
    {
        public List<int> LikedStages
        {
            get;
            set;
        }

        public List<int> DislikedStages
        {
            get;
            set;
        }
        
        public List<int> ForbiddenStages
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            LikedStages = ((List<object>)dictionary["Like"]).Cast<int>().ToList();
            DislikedStages = ((List<object>)dictionary["Dislike"]).Cast<int>().ToList();
            ForbiddenStages = ((List<object>)dictionary["Forbidden"]).Cast<int>().ToList();
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}