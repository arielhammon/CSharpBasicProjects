using System;

namespace ClassMethodsAndParameters
{
    class Program
    {
        static void Main(string[] args)
        {
            Filler MyFiller = new Filler();
            
            //here we call a method by passing literals
            MyFiller.FillMultiples(5, 20);

            //now here we call the same method by passing variables using named parameters
            int m = 7;
            int n = 6;
            MyFiller.FillMultiples(firstNum: m, howMany: n);

            Console.ReadLine();
        }
    }
}
