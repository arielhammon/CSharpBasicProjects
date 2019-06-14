using System;
using System.Collections.Generic;
using System.Text;

namespace ClassMethods
{
    public static class MathOps
    {
        static public int TwoToPowerOf(int n)
        //returns 2^n
        {
            int nn = 1;
            for (int i = 1; i < n; i++)
            {
                checked { nn *= 2; }
            }
            return nn;
        }
        static public int Factorial(int n)
        //returns n! = n(n-1)(n-2)*...*3*2*1
        {
            int nn = 1;
            for (int i = 1; i < n + 1; i++)
            {
                checked { nn *= i; }
            }
            return nn;
        }

        static public int RaiseToPowerOfSelf(int n)
        //returns n^n
        {
            int nn = 1;
            for (int i = 0; i < n; i++)
            {
                checked { nn *= n; }
            }
            return nn;
        }
    }
}
