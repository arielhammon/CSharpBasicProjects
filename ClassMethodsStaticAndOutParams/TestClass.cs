using System;
using System.Collections.Generic;
using System.Text;

namespace ClassMethodsStaticAndOutParams
{
    class TestClass
    {
        public static int AnswerToEverything()
        {
            return 42;
        }
        public void DivideBy2(int a, out int result)
        {
            result = a / 2;
        }

        public void DivRemainder(int a, int b, out int quotient, out int remainder)
        {
            quotient = a / b;
            remainder = a % b;
        }

        public void DivRemainder(long a, long b, out long quotient, out long remainder)
        {
            quotient = a / b;
            remainder = a % b;
        }
    }
}
