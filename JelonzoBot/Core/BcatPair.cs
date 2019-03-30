namespace JelonzoBot.Core
{
    public class BcatPair
    {
        public string TitleId
        {
            get;
        }

        public string Passphrase
        {
            get;
        }

        public BcatPair(string tid, string passphrase)
        {
            TitleId = tid;
            Passphrase = passphrase;
        }
        
    }
}