using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string[] strArray = new string[] { "Horatio", "Sally", "Fred", "Manny", "Yvonne", "Kendra" };
        Console.WriteLine("We have an array of names. Let's say something about them.");
        Console.WriteLine("Please enter something about John, like \"John is a rockstar.\" or \"I think John is cool.\":");
        string txt = Console.ReadLine();
        string left = ""; //the first part of the user's sentence, split by "John"
        string right = ""; //the second part of the user's sentence, split by "John"
        int pos = txt.ToLower().IndexOf("john");
        if (pos >= 0)
        {
            left = txt.Substring(0, pos);
            right = txt.Substring(pos + 4, txt.Length - pos - 4);
        }
        else
        {
            Console.WriteLine("We couldn't find \"John\", but we'll go with what you gave us.");
        }
        int i, j; //counters
        for (i=0; i<strArray.Length; i++)
        {
            strArray[i] = left + strArray[i] + right;
        }
        for (i=0 ; i<strArray.Length; i++) //separate loop, because burning real estate
        {
            Console.WriteLine(strArray[i]);
        }

        //for (i = 0; i != -1; i++)
        //{
        //    Console.WriteLine("This is an infinite loop. Oops!");
        //}

        for (i=0; i<0; i++)
        {
            Console.WriteLine("This sentence won't be printed at all.");
        }

        for (i=0; i<2; i++)
        {
            Console.WriteLine("This sentence will be repeated twice.");
        }

        for (i = 0; i <= 2; i++)
        {
            Console.WriteLine("This sentence will be repeated thrice.");
        }

        List<string> strList = new List<string>();
        string temp = "";
        for (i=0; i<10; i++)
        {
            temp += i;
            Console.WriteLine(temp);
            strList.Add(temp);
        }
        Console.Write("These strings are in a list where each item is unique. Please type some text to search for in the list:  ");
        temp = Console.ReadLine();
        int index = -1;
        for (i = 0; i < strList.Count; i++)
        {
            if (temp == strList[i])
            {
                index = i;
                Console.WriteLine("That text was found at index {0}.", index);
                break;
            }
        }
        if (index == -1)
        {
            Console.WriteLine("I'm sorry, that text was not found in the list.");
        }
        index = strList.IndexOf(temp);
        if (index>=0)
        {
            Console.WriteLine("The built-in method also found the text at index {0}.", index);
        }
        else
        {
            Console.WriteLine("I'm sorry, the built-in method also couldn't find that text in the list.");
        }
        strList.Clear();
        temp = ""; //done on the first iteration, but initialized here for safety
        for (i = 0; i < 12; i++)
        {
            if (i % 3 == 0)
            {
                temp = "";
            }
            temp += 0;
            Console.WriteLine(temp);
            strList.Add(temp);
        }
        Console.Write("These strings are in a list of non-unique items. Please type some text to search for in the list:  ");
        temp = Console.ReadLine();
        index = -1;
        for (i = 0; i < strList.Count; i++)
        {
            if (temp == strList[i])
            {
                index = i;
                Console.WriteLine("That text was found at index {0}.", index);
            }
        }
        if (index == -1)
        {
            Console.WriteLine("I'm sorry, that text was not found in the list.");
        }
        Console.WriteLine();
        Console.WriteLine("Now we'll repeat that list and count prior occurances of each string:");
        int count;
        for (i = 0; i < strList.Count; i++)
        {
            count = 0;
            Console.Write(strList[i]);
            for (j = 0; j < i; j++)
            {
                if (strList[j] == strList[i])
                {
                    count += 1;
                }
            }
            switch (count)
            {
                case 0:
                    Console.WriteLine("\tThis string has NOT already appeared in the list.");
                    break;
                case 1:
                    Console.WriteLine("\tThis string has already appeared in the list once.");
                    break;
                default:
                    Console.WriteLine("\tThis string has already appeared in the list {0} times.", count);
                    break;
            }
        }
        Console.WriteLine("That's all folks! Have a great day.");
        Console.ReadLine();
    }
}
