using System;

namespace ConstructorChaining
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new RandomByType();
            Console.WriteLine("Here's a random 8 bit integer: " + random.ByteNext);
            Console.WriteLine("Here's a random 16 bit integer: " + random.ShortNext);
            Console.WriteLine("Here's a random 16 bit unsigned integer: " + random.UShortNext);
            Console.WriteLine("Here's a random 32 bit integer: " + random.IntNext);
            Console.WriteLine("Here's a random 32 bit unsigned integer: " + random.UIntNext);
            Console.WriteLine("Here's a random 64 bit integer: " + random.LongNext);
            Console.WriteLine("Here's a random 64 bit integer: " + random.ULongNext);
            Console.WriteLine("Here's a random boolean: " + random.BoolNext);
            Console.WriteLine("Here's a random 64 bit float: " + random.DoubleNext);
            Console.WriteLine("Here's a random 32 bit float: " + random.FloatNext);

            Console.ReadLine();
        }
    }
}
