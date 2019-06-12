using System;
using System.Text;

namespace Strings
{
    class Program
    {
        static void Main(string[] args)
        {
            //demonstrating that int is a value type
            int i, j;
            i = 5;
            j = i;
            j = j + 5; //avoiding compound operator for demo purposes
            Console.WriteLine("i = " + i + ". j = " + j + ".");
            Console.WriteLine("See? i and j are different.");
            Console.WriteLine();

            //demonstrating that string, which is a reference type, behaves like int--meaning it is immutable
            string s, t;
            s = "abc";
            t = s;
            t = t + "def"; //avoiding compound operator for demo purposes
            Console.WriteLine("s = " + s + ". t = " + t + ".");
            Console.WriteLine("See? s and t are different. t refers to a new instance of string after concatenation.");
            Console.WriteLine();

            //demonstrating that stringbuilder, which is also a reference type, is mutable
            StringBuilder sb = new StringBuilder();
            StringBuilder tb;
            sb.Append("abc");
            tb = sb;
            tb.Append("def");
            Console.WriteLine("sb = " + sb.ToString() + ". tb = " + tb.ToString() + ".");
            Console.WriteLine("See? sb and tb are the same. Changing one changes the other because they refer to the same instance.");
            Console.WriteLine();

            //other string operations
            string fName = "Johann Carl";
            string mName = "Friedrich";
            string lName = "Gauss";
            string fullName = fName + " " + mName + " " + lName.ToUpper();
            string lineOut = "THE prince of mathematicians is " + fullName + ".";
            Console.WriteLine("THE prince of mathematicians is " + fullName + ".");
            //check to see if lineOut contains the words "prince" and "gauss" without respect to case
            Console.WriteLine(lineOut.ToLower().Contains("prince") && lineOut.ToLower().Contains("gauss"));
            //same thing another way
            Console.WriteLine(lineOut.Contains("prince",StringComparison.OrdinalIgnoreCase) && lineOut.Contains("gauss",StringComparison.OrdinalIgnoreCase));
            Console.WriteLine();

            //building a stringBuilder object
            StringBuilder Poem = new StringBuilder();
            Poem.Append("The Red Wheelbarrow\n");
            Poem.Append("-------------------\n");
            Poem.Append("so much depends\n");
            Poem.Append("upon\n");
            Poem.Append("\n");
            Poem.Append("a red wheel\n");
            Poem.Append("barrow\n");
            Poem.Append("\n");
            Poem.Append("glazed with rain\n");
            Poem.Append("water\n");
            Poem.Append("\n");
            Poem.Append("beside the white\n");
            Poem.Append("chickens\n");
            Poem.Append("\t-William Carlos Williams");
            Console.WriteLine(Poem);

            Console.ReadLine();
        }
    }
}
