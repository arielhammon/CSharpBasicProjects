using System;
using System.IO;

namespace FileIO
{
    class Program
    {
        enum NumberType { intNum, decNum, nullNum }
        static void Main(string[] args)
        {
            int count = 0;
            NumberType nType = NumberType.nullNum;
            long longResult = 0;
            double doubleResult = 0;
            do
            {
                Console.Write("Please enter a number:  ");
                string response = Console.ReadLine();
                if (long.TryParse(response, out longResult))
                {
                    Console.WriteLine("Okay, got it. Thanks. Looks like you entered an integer.");
                    nType = NumberType.intNum;
                    count = 3;
                }
                else if (double.TryParse(response, out doubleResult))
                {
                    Console.WriteLine("Okay, got it. Thanks. Looks like you entered a decimal number.");
                    nType = NumberType.decNum;
                    count = 3;
                }
                else
                {
                    count++;
                    if (count == 3)
                    {
                        Console.WriteLine("Sorry, we didn't recognize your entry.");
                        Console.WriteLine("For demo purposes, we'll use 7813.");
                        longResult = 7813;
                    }
                    else
                    {
                        Console.WriteLine("Sorry, we didn't recognize your entry. Please try again.");
                    }
                }
            } while (count < 3);
            Console.WriteLine("We will now append your number to a file.");
            string output;
            switch (nType)
            {
                case NumberType.intNum:
                    output = longResult.ToString();
                    break;
                case NumberType.decNum:
                    output = doubleResult.ToString();
                    break;
                default:
                    output = "";
                    break;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter("numbers.log", append: true)) //file will be located in the debug folder if running in debug mode
                {
                    writer.WriteLine(output);
                }
                Console.WriteLine("Success!");
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, something went wrong. The number was not written to any file.");
            }
            Console.WriteLine("We will now print the contents of the file:");
            string contents;
            try
            {
                using (StreamReader reader = new StreamReader("numbers.log"))
                {
                    contents = reader.ReadToEnd();
                }
                Console.WriteLine(contents);
            }
            catch(Exception)
            {
                Console.WriteLine("Sorry, something went wrong. We were not able to read the file.");
            }
            Console.WriteLine("We hope you enjoyed this demo. Come again!");
            Console.ReadLine();
        }
    }
}
