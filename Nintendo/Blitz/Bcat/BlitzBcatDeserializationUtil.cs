using System;

namespace Nintendo.Blitz.Bcat
{
    public class BlitzBcatDeserializationUtil
    {
        public static DateTime DeserializeDateTime(object obj)
        {
            // Get the string
            string str = (string)obj;

            // Check if it doesn't end with the time zone already
            if (!str.EndsWith("+00:00"))
            {
                str += "+00:00";   
            }

            // Parse and return as UTC time
            return DateTime.Parse(str).ToUniversalTime();
        }

        public static float GetValueAsFloat(object obj)
        {
            return (obj as float?) ?? Convert.ToSingle(obj);
        }
        
    }
}