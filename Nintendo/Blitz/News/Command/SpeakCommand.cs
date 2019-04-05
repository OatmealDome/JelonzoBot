namespace Nintendo.Blitz.News.Command
{
    public class SpeakCommand : ScriptCommand
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

        public CameraType CameraType
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public bool Skip
        {
            get;
            set;
        }

        public bool WaitButton
        {
            get;
            set;
        }

        public bool IsEndImm
        {
            get;
            set;
        }

        public bool IsNoInterval
        {
            get;
            set;
        }

    }
}