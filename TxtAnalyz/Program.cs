using System;
using VerbBuilder;

namespace TextAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            string sUsrText = "";
            while (true)
            {
                Console.WriteLine("Enter your data to predict : ");
                sUsrText = Console.ReadLine();
                if (sUsrText == "exit" || sUsrText == "0")
                    break;
                Predict(sUsrText);
            }
            //sUsrText = "I are a man";
            //Predict(sUsrText);
        }

        private static void Predict(string sUserText)
        {
            CanProcessor process = new CanProcessor();
            process.ReadData(@"C:\NEWHOME\RandD\TxtAnalyz\TxtAnalyz\Data.csv");
            int iValue = process.IsValid(sUserText);
            if (iValue == 0)
                Console.WriteLine("Possibly a Fact / Error!");
            else if (iValue == 1)
                Console.WriteLine("Possible!");
            else
                Console.WriteLine("Error!");
        }
    }
}
