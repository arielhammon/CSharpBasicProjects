using System;

namespace ExceptionHandlingWithInheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to out exception handling demo.");
            Console.WriteLine("We will be handling exceptions, but also creating custom exception classes.");
            int tries = 0;
            entryPoint:
            Console.Write("Please enter your age in years: ");
            string temp = Console.ReadLine();
            int age;
            try
            {
                tries++;
                age = int.Parse(temp);
                if (age < 0)
                {
                    var ex = new NegativeAgeException("Age cannot be negative.");
                    ex.Source = "Program.Main";
                    ex.HelpLink = "http://somesite.com/errors/somedoc.html";
                    throw ex;
                }
                if (age > 150)
                {
                    var ex = new OverAgeException("Age cannot be over 150 years.");
                    ex.Source = "Program.Main";
                    ex.HelpLink = "http://somesite.com/errors/someotherdoc.html";
                    throw ex;
                }   
            }
            catch (FormatException ex)
            {
                ex.HelpLink = "http://somesite.com/errors/someotherotherdoc.html";
                if (tries < 3)
                {
                    Console.WriteLine("Please try again and enter a whole number in years.");
                    goto entryPoint;
                }
                else
                {
                    Console.WriteLine("Please see {0} for additional help with this problem.", ex.HelpLink);
                    Console.ReadLine();
                    return;
                }
            }
            catch (NegativeAgeException ex)
            {
                if (tries < 3)
                {
                    Console.WriteLine("Please try again and enter a positive whole number in years.");
                    goto entryPoint;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please see {0} for additional help with this problem.", ex.HelpLink);
                    Console.ReadLine();
                    return;
                }
            }
            catch (OverAgeException ex)
            {
                if (tries < 3)
                {
                    Console.WriteLine("Please try again and enter a whole number under 150 in years.");
                    goto entryPoint;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please see {0} for additional help with this problem.", ex.HelpLink);
                    Console.ReadLine();
                    return;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("We're sorry, there was a problem. Please restart the application and try again.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Oh, you're {0} years old. Congratulations. You are at the age of glory.", age);
            Console.ReadLine();
        }
    }
}
