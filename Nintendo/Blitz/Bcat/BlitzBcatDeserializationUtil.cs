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

        public static object GetEnumValueFromString(Type enumType, string str)
        {
            object value;
            if (Enum.TryParse(enumType, str, out value))
            {
                return value;
            }
            else if (Enum.TryParse(enumType, $"c{str}", out value))
            {
                return value;
            }

            throw new Exception("Unknown enum value " + str + " for " + enumType.Name);
        }
        
    }
}