using System;

namespace ClassInheritanceAbstractAndVirtual
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee emp1 = new Employee() { FirstName = "Sample", LastName = "Student", ID = 1234, Certs = "MCSD" };
            emp1.SayName();
            Console.WriteLine("Professionally known as: " + emp1.GetFullName());
            Employee emp2 = new Employee() { FirstName = "Sample", LastName = "Professor", ID = 1235, Certs = "PhD" };
            emp2.SayName();
            Console.WriteLine("Professionally known as: " + emp2.GetFullName());
            Console.WriteLine("These two employees are the same: {0}.", emp1 == emp2);
            Employee emp3 = emp1 + emp2;
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
