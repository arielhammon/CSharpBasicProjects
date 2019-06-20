using System;

namespace ClassInheritanceAbstractAndVirtual
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee emp = new Employee() { FirstName = "Sample", LastName = "Student", ID = 1234, Certs = "MCSD" };
            emp.SayName();
            Console.WriteLine("Professionally known as: " + emp.GetFullName());
            Console.ReadLine();
        }
    }
}
