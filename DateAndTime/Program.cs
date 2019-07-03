using System;

namespace DateAndTime
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to our basic date and time demo.");
            Console.WriteLine("It is currently: " + DateTime.Now + " local time.");
            Console.WriteLine("Let's set a timer.");
            Console.Write("How many hours from now?: ");
            string temp = Console.ReadLine();
            int hours = int.Parse(temp);
            Console.Write("How many minutes from now?: ");
            temp = Console.ReadLine();
            int minutes = int.Parse(temp);
            Console.Write("How many seconds from now?: ");
            temp = Console.ReadLine();
            int seconds = int.Parse(temp);

            double milliSeconds = 1000*(hours * 3600 + minutes * 60 + seconds);
            TimeSpan timeLength = TimeSpan.FromMilliseconds(milliSeconds);
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.Add(timeLength);
            Console.WriteLine("Timer started at: " + DateTime.Now + " local time.");
            LocalTimer timer = new LocalTimer(endTime); //declares a new instance of our custom timer class

            Console.WriteLine("Please wait to be notified or enter \"cancel\" to cancel the timer.");
            temp = Console.ReadLine();
            if (temp == "cancel" && timer.active)
            {
                timer.active = false;
                Console.WriteLine("Bye!");
                Console.ReadLine();
            }
        }
    }
}
