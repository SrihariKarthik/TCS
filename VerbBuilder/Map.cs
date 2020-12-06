namespace VerbBuilder
{
    public class Map
    {
        private string subject;
        private string subjvalue;
        private string type;

        public string Subject { get => subject; set => subject = value; }
        public string Subjvalue { get => subjvalue; set => subjvalue = value; }
        public string Type { get => type; set => type = value; }

        public Map()
        {
            subject = "";
            subjvalue = "";
            type = "";
        }
        public Map(string sub, string subval, string type)
        {
            subject = sub;
            subjvalue = subval;
            this.type = type;
        }
    }
}
