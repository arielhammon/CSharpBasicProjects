using System;

namespace ConstructorChaining
{
    class RandomByType
    {
        //These constants represent the number of bytes in x bits
        const int bits8 = 1;
        const int bits16 = 2;
        const int bits32 = 4;
        const int bits64 = 8;

        private Random random;
        public RandomByType(int seed)
        {
            random = new Random(seed);
        }
        public RandomByType() : this(DateTime.Now.GetHashCode()) { }

        public int IntNext
        {
            get
            {
                var bytes = new byte[bits32];
                random.NextBytes(bytes);
                return BitConverter.ToInt32(bytes, startIndex: 0);
            }
        }
        public int IntNextMinMax(int min, int max)
        {
            return random.Next(min, max);
        }
        public int IntNextMax(int max)
        {
            return random.Next(max);
        }
        public uint UIntNext
        {
            get
            {
                var bytes = new byte[bits32];
                random.NextBytes(bytes);
                return BitConverter.ToUInt32(bytes, startIndex: 0);
            }
        }
        public long LongNext
        {
            get
            {
                var bytes = new byte[bits64];
                random.NextBytes(bytes);
                return BitConverter.ToInt64(bytes, startIndex: 0);
            }
        }
        public ulong ULongNext
        {
            get
            {
                var bytes = new byte[bits64];
                random.NextBytes(bytes);
                return BitConverter.ToUInt64(bytes, startIndex: 0);
            }
        }
        public short ShortNext
        {
            get
            {
                var bytes = new byte[bits16];
                random.NextBytes(bytes);
                return BitConverter.ToInt16(bytes, startIndex: 0);
            }
        }
        public ushort UShortNext
        {
            get
            {
                var bytes = new byte[bits16];
                random.NextBytes(bytes);
                return BitConverter.ToUInt16(bytes, startIndex: 0);
            }
        }
        public byte ByteNext
        {
            get
            {
                var bytes = new byte[bits8];
                random.NextBytes(bytes);
                return bytes[0];
            }
        }
        public double DoubleNext
        {
            get
            {
                var bytes = new byte[bits64];
                random.NextBytes(bytes);
                return BitConverter.ToDouble(bytes, startIndex: 0);
            }
        }
        public double DoubleNext0to1
        {
            get
            {
                return random.NextDouble();
            }
        }
        public float FloatNext
        {
            get
            {
                var bytes = new byte[bits32];
                random.NextBytes(bytes);
                return BitConverter.ToSingle(bytes, startIndex: 0);
            }
        }
        public bool BoolNext
        {
            get
            {
                var bytes = new byte[bits8];
                random.NextBytes(bytes);
                return BitConverter.ToBoolean(bytes, startIndex: 0);
            }
        }
    }
}
