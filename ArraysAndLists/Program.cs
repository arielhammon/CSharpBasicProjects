using System;
using System.Collections.Generic;

namespace ArraysAndLists
{
    class Program
    {
        private static uint GetUIntNumericValue(int attempts = 3, uint valueIfParseFails = 0)
        {
            Console.Write("Enter any whole number:  ");
            string temp = "";
            uint uintNum = valueIfParseFails;
            int n = 0; //the number of times the user has entered unrecognized values
            bool validEntry = false; //used to repeat opportunities to enter valid data
            while (!validEntry & n < attempts)
            {
                temp = Console.ReadLine();
                validEntry = uint.TryParse(temp, out uintNum);
                if (!validEntry)
                {
                    n++;
                    if (n < attempts)
                    {
                        Console.Write("Sorry, we had trouble recognizing that as a whole number. Please try again:  ");
                    }
                    else
                    {
                        Console.WriteLine("Sorry, for demonstration purposes, we'll record that as the number {0}.", valueIfParseFails);
                        uintNum = valueIfParseFails; //set again because TryParse() converts "" to 0 even though it returns a false value
                    }
                }
            }
            return uintNum;
        }
        static void Main(string[] args)
        {
            string[] strArray = new string[] { "Happy ", "birthday", ", ", "Bartholomew", "! ", "How ", "old ", "are ", "you", "?" };
            int nameIndex = 3;
            int[] intArray = new int[] { 1, 5, 7, 11, 15, 17, 21 };

            Console.WriteLine("Welcome to a basic exploration of arrays and lists.");
            Console.WriteLine("The following message is generated from an array of strings:");
            foreach (string element in strArray)
            {
                Console.Write(element);
            }
            Console.WriteLine();
            Console.Write("Which element of that array (0 to {0}) would you like to see? ", strArray.Length-1);
            uint i = GetUIntNumericValue(3, 0);
            if (i<strArray.Length)
            {
                Console.WriteLine("That element is: " + strArray[i] + ".");
            }
            else
            {
                Console.WriteLine("I'm sorry, that index is out of range. Trying to access a non-existent element would result in a nasty error.");
            }
            Console.WriteLine("We also have an array of possible ages for {0}:", strArray[nameIndex]);
            foreach (int ii in intArray)
            {
                Console.Write(ii + " ");
            }
            Console.WriteLine();
            Console.Write("Which element of that array (0 to {0}) would you like to assign as {1}'s age? ", intArray.Length-1, strArray[nameIndex]);
            i = GetUIntNumericValue(3, 0);
            Console.WriteLine();
            if (i < intArray.Length)
            {
                Console.WriteLine("Okay, his age is: " + intArray[i] + ".");
            }
            else
            {
                Console.WriteLine("I'm sorry, that index is out of range. Trying to access a non-existent element would result in a nasty error.");
            }
            Console.WriteLine();

            List<string> strList = new List<string>();
            foreach (string element in strArray)
            {
                strList.Add(element);
            }
            Console.WriteLine("The previous array of strings has been converted to a .Net list:");
            foreach (string element in strList)
            {
                Console.Write(element);
            }
            Console.WriteLine();
            Console.Write("Pick another element of this list (0 to {0}) that you would like to see. ", strList.Count-1);
            i = GetUIntNumericValue(3, 0);
            if (i < strList.Count)
            {
                Console.WriteLine("That element is: " + strList[(int)i] + ".");
            }
            else
            {
                Console.WriteLine("I'm sorry, that index is out of range. Trying to access a non-existent element would result in a nasty error.");
            }
            Console.WriteLine("That's all folks. Thanks for stopping by.");
            Console.ReadLine();
        }
    }
}
