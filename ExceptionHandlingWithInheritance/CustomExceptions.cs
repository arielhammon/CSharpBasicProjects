using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionHandlingWithInheritance
{
    class NegativeAgeException : Exception
    {
        public NegativeAgeException() : base() { }
        public NegativeAgeException(string message) : base(message) { }
    }

    class OverAgeException : Exception
    {
        public OverAgeException() : base() { }
        public OverAgeException(string message) : base(message) { }
    }
}
