using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee { FirstName = "Joey", LastName = "Tribbiani", ID = 1 },
                new Employee { FirstName = "Rachel", LastName = "Green", ID = 2 },
                new Employee { FirstName = "Phoebe", LastName = "Buffay", ID = 3 },
                new Employee { FirstName = "Chandler", LastName = "Bing", ID = 4 },
                new Employee { FirstName = "Ross", LastName = "Geller", ID = 5 },
                new Employee { FirstName = "Monica", LastName = "Geller", ID = 6 },
                new Employee { FirstName = "Fred", LastName = "Flintstone", ID = 7 },
                new Employee { FirstName = "Wilma", LastName = "Flintstone", ID = 8 },
                new Employee { FirstName = "Barney", LastName = "Rubble", ID = 9 },
                new Employee { FirstName = "Betty", LastName = "Rubble", ID = 10 },
                new Employee { FirstName = "Joe", LastName = "Swanson", ID = 11 },
                new Employee { FirstName = "Stewie", LastName = "Griffin", ID = 12 }
            };

            Console.WriteLine("Let's extract of list of employees named \"Joe\" using a foreach loop:");

            List<Employee> joes = new List<Employee>();
            foreach (Employee employee in employees)
            {
                if (employee.FirstName.ToLower().Contains("joe"))
                {
                    joes.Add(employee);
                }
            }
            foreach (Employee joe in joes)
            {
                Console.WriteLine(joe.FullName);
            }

            Console.WriteLine("Now, let's do the same thing using a lambda function:");
            List<Employee> joesLambda = employees.Where(x => x.FirstName.ToLower().Contains("joe")).ToList();
            foreach (Employee joe in joesLambda)
            {
                Console.WriteLine(joe.FullName);
            }

            Console.WriteLine("Now, let's use a lambda function to extract a list of employees whose ID > 5:");
            List<Employee> employeesID5 = employees.Where(x => x.ID > 5).ToList();
            foreach (Employee employee in employeesID5)
            {
                Console.WriteLine(employee.FullName + " ID: " + employee.ID);
            }

            Console.ReadLine();
        }
    }
}
