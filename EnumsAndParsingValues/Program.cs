using System;

namespace EnumsAndParsingValues
{
    class Program
    {
        enum Day
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        };
        static void Main(string[] args)
        {

            Day day;
            string response;
            string firstLetter;
            bool success = false;
            int n = 0;
            do
            {
                n++;
                try
                {
                    Console.Write("Please enter the current day of the week (spelled out fully):  ");
                    response = Console.ReadLine();
                    response = response.Trim().ToLower();
                    //Enum.Parse has an "ignorecase" parameter, but this demonstrates the process
                    //supposing that the case of certain characters needed to be controlled
                    firstLetter = response.Substring(0, 1).ToUpper();
                    response = response.Remove(0, 1);
                    response = firstLetter + response;
                    day = Enum.Parse<Day>(response);
                    Console.WriteLine("We recognized your entry as: {0}.", day);
                    success = true;
                }
                catch (ArgumentException)
                {
                    if (n < 3)
                    {
                        Console.WriteLine("Sorry, we didn't recognize your entry. Please use standard spelling.");
                    }
                    else
                    {
                        Console.WriteLine("Sorry, that didn't work out, either. We suggest runing the program again and entering something like \"Tuesday\".");
                    }
                }
            } while (!success && n < 3);
            Console.ReadLine();
        }
    }
}
