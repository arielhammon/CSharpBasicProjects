using System;

namespace LambdaFunctions
{
    class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
