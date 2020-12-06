namespace VerbBuilder
{
    public class CSV
    {
        private string col1;
        private string col2;

        public string Col1 { get => col1; set => col1 = value; }
        public string Col2 { get => col2; set => col2 = value; }

        public CSV()
        {
            col1 = "";
            col2 = "";
        }
        public CSV(string c1, string c2)
        {
            col1 = c1;
            col2 = c2;
        }

        public static CSV FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            CSV csv = new CSV();
            csv.Col1 = values[0];
            csv.Col2 = values[1];
            return csv;
        }
    }
}
