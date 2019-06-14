using System;
using System.IO;
using System.Diagnostics;

namespace ExceptionHandling
{
    class Program
    {   
        static void Main(string[] args)
        {
            //add error listener for logging errors to external file
            string filename = "errors.log"; //located in the program folder or \bin\debug folder if running in debug mode
            FileStream logFile = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            logFile.Seek(offset: 0, SeekOrigin.End); //seek to end of file
            TextWriterTraceListener textListener = new TextWriterTraceListener(logFile);
            Trace.Listeners.Add(textListener);
            Trace.AutoFlush = true;
            Trace.WriteLine("Starting Program.Main at " + DateTime.Now.ToString());
            Trace.Indent();

            bool hasError = false;
            try
            {
                int i; //counters
                int[] intArray = new int[13];
                double dblInput;

                for (i=0; i<intArray.Length; i++)
                {
                    intArray[i] = i; //populate the array
                    Console.WriteLine(i);
                }
                Console.WriteLine("We have the above list of integers. Please enter a number to divide them by:  ");
                Trace.WriteLine("Taking user input string from console and converting to double.");
                dblInput = Convert.ToDouble(Console.ReadLine());
                switch (dblInput)
                {
                    case 0:
                        //the double type allows division by zero, but it displays as NaN or infinity, which I want to dis-allow
                        throw new System.DivideByZeroException();
                    case double.NaN:
                        //the double type allows division by NaN (not a number), but it displays as NaN, which I want to dis-allow
                        throw new System.ArithmeticException("Program.Main: Let's not perform arithmetic on NaN");
                }
                
                Trace.WriteLine("Dividing each int in array by the user's double.");
                for (i=0; i<intArray.Length; i++)
                {
                    Console.WriteLine(intArray[i] / dblInput);
                }
            }
            catch (Exception ex)
            {
                hasError = true;
                //logging the error for analysis and improvement
                Trace.WriteLine("CAUGHT EXCEPTION: ");
                Trace.WriteLine(ex.ToString());
                switch (ex.GetType().Name)
                {
                    case "FormatException":
                        Console.WriteLine("Oops. Looks like you entered something that we couldn't convert to a number.");
                        break;
                    case "DivideByZeroException":
                        Console.WriteLine("Oops. Looks like you entered zero. We can't divide by zero.");
                        break;
                    case "ArithmeticException":
                        Console.WriteLine("Oops. Looks like you entered a number that we had trouble dividing by.");
                        break;
                    default:
                        Console.WriteLine("Oops. We don't know what went wrong, but we're working hard to fix it.");
                        break;
                }
            }
            finally
            {
                if (hasError)
                {                    
                    Console.WriteLine("We have logged this error in order to improve your experience. Thanks for your help!");
                    Console.WriteLine("Normally we would rock on-and-on, but this demo is complete.");
                }
                else
                {
                    Console.WriteLine("Division is awesome, yeah? No errors were encountered, so this demo is complete.");
                }

                Trace.Unindent();
                Trace.WriteLine("Ending Program.Main at " + DateTime.Now.ToString());
                Trace.WriteLine("");
                Trace.Close();
                logFile.Dispose(); //releases resources, keeps file on disk
                Console.ReadLine();
            }
        }
    }
}
