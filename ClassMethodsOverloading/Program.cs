using System;

namespace ClassMethodsOverloading
{
    class Program
    {
        static void Main(string[] args)
        {
            PrimeOps po1;
            string response;
            Console.WriteLine("Welcome to the methods overloading demo.");
            Console.WriteLine("We're going to be doing prime number calculations on large integers.");
            Console.Write("Would you like the long (about 5 mins) or the short demo? l/s:  ");
            response = Console.ReadLine().ToLower();
            if (response == "l" || response == "long")
            {
                po1 = new PrimeOps(ulong.MaxValue);
            }
            else
            {
                po1 = new PrimeOps(uint.MaxValue);
            }
            
            ulong smallestPrimeFactor;

            //===============================================================
            //demonstrate ulong overload
            //===============================================================
            smallestPrimeFactor = po1.SmallestPrimeFactor(4294967101);
            if (smallestPrimeFactor == 1)
            {
                Console.WriteLine("Congratulations, {0} is a prime number.", 4294967101);
            }
            else
            {
                Console.WriteLine("The smallest prime factor of {0} is {1}.", 4294967101, smallestPrimeFactor);
            }

            //=============================================================
            //demonstrate double overload
            //=============================================================
            smallestPrimeFactor = po1.SmallestPrimeFactor(15359831.21321);
            if (smallestPrimeFactor == 1)
            {
                Console.WriteLine("Congratulations, {0} is a prime number.", 15359831);
            }
            else
            {
                Console.WriteLine("The smallest prime factor of {0} is {1}.", 15359831, smallestPrimeFactor);
            }
            
            //=============================================================
            //demonstrate string overload
            //=============================================================
            smallestPrimeFactor = po1.SmallestPrimeFactor("10403");
            if (smallestPrimeFactor == 1)
            {
                Console.WriteLine("Congratulations, {0} is a prime number.", 10403);
            }
            else
            {
                Console.WriteLine("The smallest prime factor of {0} is {1}.", 10403, smallestPrimeFactor);
            }

            //===============================================================
            //Search for largest prime number
            //===============================================================
            if (response == "l" || response == "long")
            {
                Console.WriteLine("Now we're going to search for the largest 64 bit prime number!");
            }
            else
            {
                Console.WriteLine("Now we're going to search for the largest 32 bit prime number!");
            }
            Console.Write("Press Enter to begin:  ");
            Console.ReadLine();
            ulong i = 0;
            ulong max = po1.Number();
            ulong n;
            do
            {
                n = max - i;
                smallestPrimeFactor = po1.SmallestPrimeFactor(n);
                if (smallestPrimeFactor == 1)
                {
                    Console.WriteLine("Congratulations, {0} is a prime number.", n);
                }
                else
                {
                    Console.WriteLine("The smallest prime factor of {0} is {1}.", n, smallestPrimeFactor);
                }
                i++;
            } while (smallestPrimeFactor != 1);
            Console.WriteLine("Thanks for playing!");
            Console.ReadLine();
        }
    }
}
