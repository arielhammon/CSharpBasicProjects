using System;
using System.Collections.Generic;
using System.Text;

namespace ClassInheritanceAbstractAndVirtual
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public abstract void SayName();

        public virtual string GetFullName()
        {
            return FirstName + " " + LastName;
        }

    }
}
