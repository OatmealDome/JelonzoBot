using System;
using System.Collections.Generic;
using System.Reflection;
using Nintendo.Blitz.Bcat;
using Nintendo.Blitz.News.Command;

namespace Nintendo.Blitz.News
{
    public static class ScriptParser
    {
        public static ScriptCommand ParseCommand(Dictionary<string, object> dict)
        {
            // Try to find the type
            Type commandType = Type.GetType("Nintendo.Blitz.News.Command." + dict["Command"] + "Command");

            // Declare a variable to hold the command
            ScriptCommand command;

            // Check if no type was found
            if (commandType == null)
            {
                // Create a new generic ScriptCommand
                command = new ScriptCommand();

                // Set the command type
                command.CommandType = (CommandType)BlitzBcatDeserializationUtil.GetEnumValueFromString(typeof(CommandType), (string)dict["Command"]);

                // Return the command
                return command;
            }

            // Create a new instance of the correct ScriptCommand
            command = (ScriptCommand)Activator.CreateInstance(commandType);

            // Set the command type
            command.CommandType = (CommandType)BlitzBcatDeserializationUtil.GetEnumValueFromString(typeof(CommandType), (string)dict["Command"]);

            // Loop over every value of the command
            foreach (string key in dict.Keys)
            {
                // Skip Command
                if (key == "Command")
                {
                    continue;
                }

                // Get the property
                PropertyInfo propertyInfo = commandType.GetProperty(key);

                // Check if this is an enum
                if (propertyInfo.PropertyType.IsEnum)
                {
                    // Convert the string to the enum
                    propertyInfo.SetValue(command, BlitzBcatDeserializationUtil.GetEnumValueFromString(propertyInfo.PropertyType, (string)dict[key]));
                }
                else
                {
                    // Set directly
                    propertyInfo.SetValue(command, dict[key]);
                }
            }

            // Return the command
            return command;
        }

        public static List<ScriptCommand> ParseCommandList(List<object> list)
        {
            // Create a new ScriptCommand list
            List<ScriptCommand> commandList = new List<ScriptCommand>();

            // Loop over every Dictionary
            foreach (object obj in list)
            {
                // Cast the object
                Dictionary<string, object> dict = (Dictionary<string, object>)obj;

                // Parse the command and add it to the List
                commandList.Add(ParseCommand(dict));
            }

            // Return the command list
            return commandList;
        }

    }
}