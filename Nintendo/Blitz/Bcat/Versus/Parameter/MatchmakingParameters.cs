using System;
using System.Collections.Generic;
using System.Linq;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter
{
    [ByamlObject]
    public class MatchmakingParameters : IByamlSerializable
    {
        [ByamlMember]
        public int AddFirstMatchingTime
        {
            get;
            set;
        }

        [ByamlMember]
        public int AddMachingTime // sic
        {
            get;
            set;
        }

        [ByamlMember]
        public float BitRateMeasureRate
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public bool? KeepCleanCon
        {
            get;
            set;
        }

        [ByamlMember]
        public int TimeoutAfterJoin
        {
            get;
            set;
        }

        [ByamlMember]
        public int WaitMatchingTimes
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? cleanConCount
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? cleanConFrame
        {
            get;
            set;
        }

        public Dictionary<string, int> Indices
        {
            get;
            set;
        }

        public Dictionary<string, List<int>> IndicesShift
        {
            get;
            set;
        }

        [ByamlMember]
        public RateRanges RateRanges
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            // Create a new Indices dictionary
            Indices = new Dictionary<string, int>();

            // Get the indexes dictionary
            Dictionary<string, object> indexesDict = (Dictionary<string, object>)dictionary["Indexes"];

            // Add every index to the dictionary
            foreach (KeyValuePair<string, object> pair in indexesDict)
            {
                Indices.Add(pair.Key, (int)pair.Value);
            }

            // Create a new IndicesShift dictionary
            IndicesShift = new Dictionary<string, List<int>>();

            // Get the indexes shift dictionary
            Dictionary<string, object> indexesShift = (Dictionary<string, object>)dictionary["IndexesShift"];

            // Add every shift to the dictionary
            foreach (KeyValuePair<string, object> pair in indexesShift)
            {
                // Get the inner list
                List<object> shift = (List<object>)pair.Value;

                // Convert the list and add it to the dictionary
                IndicesShift.Add(pair.Key, shift.Cast<int>().ToList());
            }
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}