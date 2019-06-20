using System;

namespace ClassMethodsOptionalParameters
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass tc = new TestClass();

            string temp;
            int a, b, result;

            Console.WriteLine("We'll be performing a mystery operation. See if you can figure it out.");
            Console.Write("Enter any integer:  ");
            temp = Console.ReadLine();
            if (!int.TryParse(temp, out a))
            {
                Console.WriteLine("Sorry, that was not a recognized entry. For demo purposes, we'll use 5.");
                a = 5;
            }
            Console.Write("Enter another integer (optional):  ");
            temp = Console.ReadLine();
            if (temp == "")
            {
                result = tc.MultiplyAdd(a);
            }
            else
            {
                if (!int.TryParse(temp, out b))
                {
                    Console.WriteLine("Sorry, that was not a recognized entry. For demo purposes, we'll use 6.");
                    b = 6;
                }
                result = tc.MultiplyAdd(a, b);
            }

            Console.WriteLine("The result is {0}.", result);
        }
    }
}
