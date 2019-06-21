using System;
using System.Collections.Generic;

namespace ClassInheritanceAbstractAndVirtual
{
    public class Employee<T> : Person, IQuittable where T : IConvertible
    {

        public List<T> things { get; set; }

        public Employee()
        {
            things = new List<T>();
        }
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

        public static bool operator ==(Employee<T> emp1, Employee<T> emp2)
        {
            if (emp1 is null || emp2 is null) return false;
            return emp1.ID == emp2.ID;
        }

        public static bool operator ==(Employee<string> emp1, Employee<T> emp2)
        {
            return (Employee<T>)emp1 == emp2;
        }

        public static bool operator ==(Employee<int> emp1, Employee<T> emp2)
        {
            return (Employee<T>)emp1 == emp2;
        }

        public static bool operator!= (Employee<T> emp1, Employee<T> emp2)
        {
            return !(emp1 == emp2);
        }

        public static bool operator !=(Employee<string> emp1, Employee<T> emp2)
        {
            return !(emp1 == emp2);
        }
        public static bool operator !=(Employee<int> emp1, Employee<T> emp2)
        {
            return !(emp1 == emp2);
        }

        public override bool Equals(object obj)
        {
            if ((obj is null) || !this.GetType().Equals(obj.GetType())) //in case a class is derived from this class
            {
                return false;
            }
            else
            {
                Employee<T> emp = (Employee<T>)obj; //again in case a class is derived from this class
                return ID == emp.ID;
            }
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public static Employee<T> operator+ (Employee<T> emp1, Employee<T> emp2)
        //returns a new employee with an ID one greater than that of the argument with the largest ID
        //the new employee has the combined certifications of the arguments
        //I know this is kind of lame, especially since it doesn't guarantee uniqueness of ID, but it's just for demo purposes :)
        {
            int id = Math.Max(emp1.ID, emp2.ID) + 1;
            string certs = emp1.Certs + ", " + emp2.Certs;
            Employee<T> emp = new Employee<T>() { ID = id, Certs = certs };
            return emp;
        }

        private static T MyChangeType(object fromThing, Type toType)
        {
            if (fromThing.GetType() == toType) return (T)fromThing;
            try
            {
                return (T)Convert.ChangeType(fromThing, toType);
            }
            catch (InvalidCastException)
            {
                return default;
            }
            catch (FormatException)
            {
                return default;
            }
        }

        public static implicit operator Employee<T>(Employee<int> emp1)
        {
            Employee<T> temp = new Employee<T>
            {
                ID = emp1.ID,
                FirstName = emp1.FirstName,
                LastName = emp1.LastName,
                Certs = emp1.Certs
            };
            foreach (int thing in emp1.things) temp.things.Add(MyChangeType(thing, typeof(T)));
            return temp;
        }

        public static implicit operator Employee<T>(Employee<string> emp1)
        {
            Employee<T> temp = new Employee<T>
            {
                ID = emp1.ID,
                FirstName = emp1.FirstName,
                LastName = emp1.LastName,
                Certs = emp1.Certs
            };
            foreach (string thing in emp1.things) temp.things.Add(MyChangeType(thing, typeof(T)));
            return temp;
        }
    }
}
