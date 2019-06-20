using System;
using System.Collections.Generic;
using System.Text;

namespace ClassInheritanceAbstractAndVirtual
{
    public class Employee : Person
    {
        public int ID { get; set; }
        public string Certs { get; set; }
        public override void SayName()
        {
            Console.WriteLine("Name: " + base.GetFullName());
        }

        public override string GetFullName()
        {
            //adds certificaitons after the full name
            return base.GetFullName() + (string.IsNullOrEmpty(Certs) ? "" : ", " + Certs);
        }
    }
}
