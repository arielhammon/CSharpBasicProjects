using System;
using System.Collections.Generic;
using System.Text;

namespace ClassMethodsOptionalParameters
{
    class TestClass
    {
        public int MultiplyAdd (int a, int b = 1)
        {
            return a * b + a;
        }
    }
}
