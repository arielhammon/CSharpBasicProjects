using System;

namespace ClassMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            string play = "y";
            do
            {
                try
                {
                    Console.Write("Let's do some math. Enter a whole number. I recommend keeping it small:  ");
                    n = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("2^{0} (exponentiation) is: {1}.", n, MathOps.TwoToPowerOf(n));
                    Console.WriteLine("{0}! (factorial) is: {1}.", n, MathOps.Factorial(n));
                    Console.WriteLine("{0}^{0} (exponentiation) is: {1}.", n, MathOps.RaiseToPowerOfSelf(n));
                    Console.WriteLine("See, these mathematical operations grow quickly!");
                    Console.Write("Wanna go again? y/n:  ");
                    play = Console.ReadLine();
                    Console.WriteLine();
                }
                    
                catch (OverflowException ex)
                {
                    Console.WriteLine("Sorry, the number was too big to complete the calculation!");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Sorry, the input was not a whole number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (play != "n");
        }
    }
}
