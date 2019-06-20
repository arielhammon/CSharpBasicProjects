using System;

namespace ClassInheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee emp = new Employee() { FirstName = "Sample", LastName = "Student", ID = 1234 };
            emp.SayName();
        }
    }
}
