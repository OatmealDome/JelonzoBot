using BcatBotFramework.Difference;

namespace JelonzoBot.Difference
{
    public class JelonzoBotDifferenceHandlerAttribute : DifferenceHandlerAttribute
    {
        public JelonzoBotDifferenceHandlerAttribute(int type, DifferenceType differenceType, int priority)
            : base(type, differenceType, priority)
        {

        }

        public JelonzoBotDifferenceHandlerAttribute(FileType type, DifferenceType differenceType, int priority) 
            : base((int)type, differenceType, priority)
        {

        }
        
    }
}