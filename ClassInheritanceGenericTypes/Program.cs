using System;

namespace ClassInheritanceAbstractAndVirtual
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee<string> emp1 = new Employee<string>() { FirstName = "Sample", LastName = "Student", ID = 1234, Certs = "MCSD" };
            emp1.things.AddRange(new string[] { "stapler", "printer", "marker board", "1" });
            emp1.SayName();
            Console.WriteLine("Professionally known as: " + emp1.GetFullName());
            Console.WriteLine("Here is a list of this employee's things:");
            foreach (string thing in emp1.things) Console.WriteLine(thing);

            Employee<int> emp2 = new Employee<int>() { FirstName = "Sample", LastName = "Professor", ID = 1235, Certs = "PhD" };
            emp2.things.AddRange(new int[] { 2, 3, 5, 7 });
            emp2.SayName();
            Console.WriteLine("Professionally known as: " + emp2.GetFullName());
            Console.WriteLine("Here is a list of this employee's things:");
            foreach (int thing in emp2.things) Console.WriteLine(thing);
            Console.WriteLine("These two employees are the same: {0}.", emp1 == emp2);

            Employee<string> emp3 = emp1 + (Employee<string>)emp2;
            emp3.FirstName = "Buzz";
            emp3.LastName = "Lightyear";
            emp3.SayName();
            Console.WriteLine("This employee has certifications: {0}.", emp3.Certs);

            Console.WriteLine();
            Console.WriteLine("Update:");
            emp1.Quit();
            Console.ReadLine();
        }
    }
}
