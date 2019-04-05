namespace Nintendo.Blitz.News.Command
{
    public class WaitAnimCommand : ScriptCommand
    {
        public Speaker Speaker
        {
            get;
            set;
        }

        public int MinWait
        {
            get;
            set;
        }

    }
}