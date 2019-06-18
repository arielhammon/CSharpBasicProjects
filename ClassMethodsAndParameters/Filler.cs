using System;
using System.Collections.Generic;

namespace ClassMethodsAndParameters
{
    class Filler
    {
        public List<int> MList { get; set; }
        public Filler()
        {
            MList = new List<int>();
        }

        public void FillMultiples(int firstNum, int howMany)
        {
            int n = firstNum;
            for (int i = 0; i < howMany; i++)
            {
                MList.Add(n);
                Console.Write(n + " ");
                n += firstNum;
            }
            Console.WriteLine();
            Console.WriteLine("We have added {0} multiples of {1} to the list.", howMany, firstNum);
        }
    }
}
