using System;
using System.Collections.Generic;
using System.Text;

namespace ClassInheritanceAbstractAndVirtual
{
    public class Employee : Person, IQuittable
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

        public void Quit()
        {
            string message;
            message = "Employee ID: " + ID + "\n";
            message += "Name: " + GetFullName() + "\n";
            message += "Quit our grand establishment on " + DateTime.Now.ToString() + ".";
            Console.WriteLine(message);
        }
    }
}
