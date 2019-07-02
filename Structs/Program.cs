using System;

namespace NumericStructs
{
    class Program
    {
        static void Main(string[] args)
        {
            Fraction frac = new Fraction();
            Console.Write("Please enter a Decimal number: ");
            string temp = Console.ReadLine();
            if (!decimal.TryParse(temp, out decimal dec))
                Console.WriteLine("Sorry, we didn't recogize your entry");
            else
            {
                frac.DecimalValue = dec;
                Console.WriteLine("Here's your entry as a fraction: " + frac.ToString());
            }
            Console.WriteLine("Below, we demonstrate the interaction between Double and Fraction:");
            frac.SetTo(1,2);
            Console.WriteLine(frac.DoubleValue.ToString());

            frac.SetTo(1, 3);
            Console.WriteLine(frac.DoubleValue.ToString());

            double d = double.Parse("5.15151515151515E+297");
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());

            d = double.Parse("-5.15151515151515E+297"); ;
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());

            d = double.Parse("5.15151515151515E-301");
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());

            d = double.Parse("-5.15151515151515E-301");
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());

            d = double.MaxValue;
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());


            d = double.MinValue;
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());

            d = double.Epsilon;
            frac.DoubleValue = d;
            Console.WriteLine(d.ToString() + "\t" + frac.DoubleValue.ToString());

            Console.WriteLine("Below, we demonstrate various operations on fractions:");

            Fraction f1 = new Fraction(2, 3);
            Fraction f2 = new Fraction(3, 5);
            Console.WriteLine(f1.ToString() + " + " + f2.ToString() + " = " + (f1 + f2).ToString());
            Console.WriteLine(f1.ToString() + " - " + f2.ToString() + " = " + (f1 - f2).ToString());
            Console.WriteLine(f1.ToString() + " * " + f2.ToString() + " = " + (f1 * f2).ToString());
            Console.WriteLine(f1.ToString() + " / " + f2.ToString() + " = " + (f1 / f2).ToString());
            Console.WriteLine(f1.ToString() + " < " + f2.ToString() + " = " + (f1 < f2).ToString());
            Console.WriteLine(f1.ToString() + " > " + f2.ToString() + " = " + (f1 > f2).ToString());
            Console.WriteLine(f1.ToString() + " <= " + f2.ToString() + " = " + (f1 <= f2).ToString());
            Console.WriteLine(f1.ToString() + " >= " + f2.ToString() + " = " + (f1 >= f2).ToString());

            Console.ReadLine();
        }
    }
}
