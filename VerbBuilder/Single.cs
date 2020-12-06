namespace VerbBuilder
{
    public class Single
    {
        private string subj;

        public string Subject { get => subj; set => subj = value; }
        public string subject { get => subj;}
        public Single()
        {
            subj = "";
        }
        public Single(string sub)
        {
            subj = sub;
        }
    }
}
