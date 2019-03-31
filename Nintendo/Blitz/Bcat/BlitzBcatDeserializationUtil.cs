using System;

namespace Nintendo.Blitz.Bcat
{
    public class BlitzBcatDeserializationUtil
    {
        public static DateTime DeserializeDateTime(object str)
        {
            return DateTime.Parse((string)(str) + "+00:00").ToUniversalTime();
        }
        
    }
}