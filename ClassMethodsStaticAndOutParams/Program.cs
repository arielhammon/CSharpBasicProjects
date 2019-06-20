using System;

namespace ClassMethodsStaticAndOutParams
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass tc = new TestClass();
            string temp;
            int a, b;
            long c, d;
            Console.Write("Enter an integer:  ");
            temp = Console.ReadLine();
            if (!int.TryParse(temp,out a))
            {
                Console.WriteLine("Sorry, we didn't recognize that entry. We'll use 48 to demonstrate.");
                a = 48;
            }
            tc.DivideBy2(a, out b);
            Console.WriteLine("The integer half of {0} is {1}.", a, b);
            tc.DivRemainder(48, 17, quotient: out a, remainder: out b);
            Console.WriteLine("If you divide 48 by 17, you get a quotient of {0} and a remainder of {1}.", a, b);
            tc.DivRemainder(48, 17, quotient: out c, remainder: out d);
            Console.WriteLine("Here's the same one with longs, demonstrating method overload:");
            Console.WriteLine("If you divide 48 by 17, you get a quotient of {0} and a remainder of {1}.", c, d);
            Console.WriteLine("This demonstrates a static method:");
            Console.WriteLine("The answer to everything is {0}.", TestClass.AnswerToEverything());
            Console.WriteLine("This demonstrates a static class:");
            Console.WriteLine("The third day of the week is {0}.", StaticClass.Day(3));
            Console.ReadLine();
        }
    }
}
