namespace Nintendo.Blitz.News.Command
{
    public class ChangeAnimationCommand : ScriptCommand
    {
        public Speaker Speaker
        {
            get;
            set;
        }

        public Emotion Emotion
        {
            get;
            set;
        }

    }
}