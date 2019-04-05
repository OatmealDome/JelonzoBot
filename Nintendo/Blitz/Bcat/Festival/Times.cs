using System;
using System.Collections.Generic;
using Nintendo.Blitz.Bcat;
using Syroot.NintenTools.Byaml.Serialization;

namespace NIntendo.Blitz.Bcat.Festival
{
    [ByamlObject]
    public class Times : IByamlSerializable
    {
        public DateTime Announcement
        {
            get;
            set;
        }

        public DateTime Start
        {
            get;
            set;
        }

        public DateTime End
        {
            get;
            set;
        }

        public DateTime Result
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            Announcement = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["Announce"]);
            Start = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["Start"]);
            End = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["End"]);
            Result = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["Result"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}