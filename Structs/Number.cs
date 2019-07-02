using System;

namespace NumericStructs
{
    struct Fraction
    {
        //Represented as Fraction = (sign)num/den * 2^exponent.
        //Total of 148 bits = 64 bits (for numerator) + 64 bits (for denominator) + 16 bits (for exponent) + 8 bits (for states)
        //This struct is a companion to the Double type offering greater precision, range, and representation of rational values.
        //For instance, Double cannot represent many values such as 1/3, 1/10, or 1/12 precisely, but Fraction can.
        //This data structure offers utilities for converting between Fraction and Double.
        //We assume that operations involving overflow will occur. In order to avoid incorrect results or throwing errors, 
        //We represent overflow values (and Double values) such as (+/-)infinity as (+/-)1/0 and NaN as 0/0.
        private ulong mNumerator; //stored as unsigned long to offer maximum range
        private ulong mDenominator; //stored as unsigned long to offer maximum range
        private short mExponent; //the exponent in 2^exponent; short places this much beyond the range of double
        private byte mStates;
        //mStates bits: 76543210
        //bit 0 sign: 1 negative, 0 positive
        //bit 1 reduced: 1 reduced, 0 notreduced
        //bit 2 exact: 1 exact, 0 not exact (some operation has caused truncation in mNumerator or mDenominator)
        //bit 3 user defined boolean flag 5
        //bit 4 user defined boolean flag 4
        //bit 5 user defined boolean flag 3
        //bit 6 user defined boolean flag 2
        //bit 7 user defined boolean flag 1

        public Fraction(ulong Numerator, ulong Denominator, int sign = 1, short exponent = 0)
        {
            mNumerator = Numerator;
            mDenominator = Denominator;
            mExponent = exponent;
            mStates = 0b00000000;
            Sign = sign;
            IsReduced = false;
            IsExact = true;
        }
        //public interfaces keep stored values private for safety and ease of future modification
        public ulong Numerator
        {

            get { return mNumerator; }

            set
            {
                mNumerator = value;
                IsReduced = false;
                IsExact = true;
            }
        }
        public ulong Denominator
        {
            get { return mDenominator; }

            set
            {
                mDenominator = value;
                IsReduced = false;
                IsExact = true;
            }
        }
        public int Sign
        {
            get
            {
                if (mNumerator == 0) return 1; //ensures that 0 or NaN will always be treated as positive, even if manually set to negative
                if ((mStates & 0b00000001) == 0) return 1; else return -1;
            }
            set
            {
                if (value < 0) mStates |= 0b00000001; else mStates &= 0b11111110;
            }
        }

        public bool IsReduced
        {
            get
            {
                if ((mStates & 0b00000010) == 0) return false; else return true;
            }
            private set
            {
                if (value) mStates |= 0b00000010; else mStates &= 0b11111101;
            }
        }

        public bool IsExact
        {
            get
            {
                if ((mStates & 0b00000010) == 0) return false; else return true;
            }
            set
            {
                if (value) mStates |= 0b00000100; else mStates &= 0b11111011;
            }
        }
        public bool GetUserFlag(int index0to4)
        {
            if (index0to4 > 4 || index0to4 < 0) throw new ArgumentOutOfRangeException("Argument index0to4 must be equal to 0, 1, 2, 3 or 4 only.");
            byte selector = (byte)(0b1 << index0to4 + 3);
            return ((mStates & selector) == 0 ? false : true);
        }

        public void SetUserFlag(int index0to4, bool value)
        {
            if (value)
            {
                byte flag = (byte)(0b00000001 << index0to4 + 3);
                mStates |= flag;
            }
            else
            {
                byte flag = (byte)((0b00000001 << index0to4 + 3) ^ 0b11111111);
                mStates &= flag;
            }
        }
        public short Exponent { get { return mExponent; } set { mExponent = value; } }
        public static Fraction NaN { get { return new Fraction(0, 0, 1, 0); } }
        public static Fraction PositiveInfinity { get { return new Fraction(1, 0, 1, 0); } }
        public static Fraction NegativeInfinity { get { return new Fraction(1, 0, -1, 0); } }
        public bool IsPosInfinity
        {
            get { if (mDenominator == 0 && mNumerator != 0 && Sign > 0) return true; else return false; }
        }
        public bool IsNegInfinity
        {
            get { if (mDenominator == 0 && mNumerator != 0 && Sign < 0) return true; else return false; }
        }
        public bool IsNaN
        {
            get { if (mDenominator == 0 && mNumerator == 0) return true; else return false; }
        }
        public bool IsFinite
        {
            get { if (mDenominator != 0) return true; else return false; }
        }
        public double DoubleValue
        {
            get
            {
                //see the set method below for a full description of the Double data type
                //set the sign
                long dbits = 0;
                if (Sign < 0) dbits = 0b1L << 63;
                if (mDenominator == 0)
                {
                    if (mNumerator == 0) return double.NaN;
                    if (dbits < 0) return double.NegativeInfinity; else return double.PositiveInfinity;
                }
                ulong qbits = QuotientBits(mNumerator, mDenominator, out int exponent, out int sigbit);
                exponent += 1075; //bias the exponent (see set method): exponent = exponent + 1023 (standard bias) + 52 (compensate for whole number)
                exponent += mExponent; //apply the main exponent from Fraction
                //shift qbits so that most significant 1 is in bit 52
                int shift = 52 - sigbit;
                if (shift > 0)
                {
                    qbits <<= shift;
                    exponent -= shift;
                }
                else
                {
                    shift = -shift;
                    qbits >>= shift;
                    exponent += shift;
                }
                //drop (do not store) the leading 1 (see set method)
                ulong mantissaSelector = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111ul; //mantissa stored in 52 bits
                ulong mantissa = qbits & mantissaSelector;

                if (exponent < 0) //subnormal number, increase exponent as much as possible
                {
                    qbits >>= 1; //recover dropped most significant 1
                    do
                    {
                        qbits >>= 1;
                        mantissa = qbits & mantissaSelector;
                        exponent++;
                    } while (exponent < 0 && mantissa != 0);
                }
                if (exponent < 0) return 0.0;
                if (exponent > 2047)
                {
                    if (dbits < 0) return double.NegativeInfinity; else return double.PositiveInfinity;
                }
                dbits |= (long)exponent << 52;
                dbits |= (long)mantissa;
                return BitConverter.Int64BitsToDouble(dbits);
            }

            set
            {
                double d = value;
                //see below for the IEEE 754 double encoding standards which this conversion algorithm is based on
                //https://en.wikipedia.org/wiki/Double-precision_floating-point_format#Java
                //for normal doubles: d = (-1)^sign x 2^(e-1023) * 1.fraction, e = biased exponent, mantissa = 1.fraction
                //for subnormal doubles: d = (-1)^sign x 2^(1-1023) * 0.fraction, e = 0 + 1, mantissa = 0.fraction

                //extract the sign, exponent, and mantissa.
                long bits = BitConverter.DoubleToInt64Bits(d);
                //the sign of double is stored in the 63rd bit (0 based): 1 negative; 0 positive
                if ((bits & (0b1L << 63)) == 0) Sign = 1; else Sign = -1;
                //the exponent of double is stored in 11 bits, bits 52-62 (0 based)
                int exponent = (int)((bits >> 52) & 0b0111_1111_1111L);
                //the mantissa of double is stored in 52 bits, bits 0-52 (0 based)
                ulong mantissa = (ulong)(bits & 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L);

                switch (exponent)
                {
                    case 0:
                        if (mantissa == 0)
                        {
                            SetToZero();
                            return;
                        }
                        //the double is subnormal
                        //subnormal doubles have an exponent of 0 and follow slighly different
                        //conventions in order to allow for smaller representations of floating point numbers (closer to zero)
                        exponent++;
                        break;
                    case 0b0111_1111_1111:
                        //the maximum exponent is reserved to represent infinity, -infinity, or NaN
                        //we represent (+/-)infinity as (+/-)1/0 and NaN as 0/0
                        if (mantissa == 0) SetTo(1, 0, Sign, 0) /*(+/-)infinity*/; else SetTo(0, 0, 1, 0); /*NaN*/
                        return;
                    default:
                        //the double is normal
                        //since all normal binary representations have a leading 1,
                        //the IEEE 754 standard specifies an unstored 1 in bit 52 (0 based)
                        mantissa |= 0b1L << 52;
                        break;
                }

                //unbias the exponent
                //the IEEE 754 standard specifies an exponent biased by +1023 and a mantissa of 1.fraction
                //but we're treating the mantissa as a whole number: 1.fraction * 2^52
                //hence, we need to subtract an additional 52 from the exponent to compensate
                exponent -= 1075; //exponent = exponent - 1023 - 52

                //remove trailing zeros (reduce the mantissa) and increase the exponent accordingly
                mantissa = ReducedULong(mantissa, out int exp);
                exponent += exp;

                mNumerator = mantissa;
                mDenominator = 1;
                mExponent = (short)exponent;
                IsExact = true;
                IsReduced = true;
            }
        }
        public decimal DecimalValue
        {
            //get {...} is not included since Decimal does not support values such as (+/-)infinity and NaN
            //thus introducing the high likelihood of throwing errors since
            //the range of Fraction is much, much greater than the range of Decimal
            //if an approximate Decimal value is desired use Convert.ToDecimal(fraction.DoubleValue)
            set
            {
                int[] bits = decimal.GetBits(value);
                if (value == 0) { SetToZero(); return; }
                IsReduced = false;
                IsExact = true;
                //the sign is stored in bit 31 (0-based) of the last integer in the bits array
                if ((bits[3] & 0b1 << 63) == 0) Sign = 1; else Sign = -1;
                //the exponent in 10^exp is stored in bits 16-23 (0-based) of the last integer in the bits array
                //represents the power of 10 which the 96 bit integer encoded in decimal is divided by
                int exp10 = (bits[3] & 0b0000_0000_1111_1111_0000_0000_0000_0000) >> 16;
                //find most significant bit in bits[2] the high bits, bits[1], the middle bits, or bits[0] the low bits
                for (int ii = 0; ii<4; ii++)
                {
                    PrintBits(bits[ii]);
                }
                int i = 3; //the bits array index
                int j = 32; //the bit int index
                bool found = false;
                while (!found)
                {
                    i--;
                    j = 32;
                    while (!found && j>=0)
                    {
                        j--;
                        if ((bits[i] & 0b1 << j) != 0) found = true;
                    } 
                }
                int k = i * 32 + j; //the ulong numerator bit index
                int exp2 = 0; //the exponent in (sign)num/den x 2^exp
                if (k>63)
                {
                    exp2 = k - 63;
                    k = 63;
                    IsExact = false;
                }
                ulong num = 0;
                while (k>=0)
                {
                    if ((bits[i] & 0b1 << j) != 0)
                    {
                        num |= 0b1ul << k;
                    }
                    k--;
                    j--;
                    if (j < 0)
                    {
                        i--;
                        j = 31;
                    }
                }
                ulong den = 1;
                if (exp10 > 19)
                {
                    //overflow occurs with 10^exp10 when exp10 > 19
                    //exp10 > 19 means that there is a residue power of 10 to resolve
                    //below, we find the power of 2 nearest to (but less than) the residue power of 10
                    //to convert a power of 10 to a power of 2, exp2 = exp10 * log_2(10)
                    //log_2(10) = 3.3219280948873623478703194294894
                    //the fractional part will be absorbed into the denominator as follows
                    //den = 10^19 * 2^0.32192809... = 10^19 * 1.25 = 12500000000000000000
                    exp2 -= exp10 * 3;
                    den = 12500000000000000000;
                }
                else
                {
                    while (exp10 > 0)
                    {
                        den *= 10;
                        exp10--;
                    }
                }
                mNumerator = num;
                mDenominator = den;
                mExponent = (short)exp2; //no worry of overflow since range of short very greatly exceeds range of exp10
            }
        }
        public ulong GCD()
        {
            return GCD(mNumerator, mDenominator);
        }
        public static ulong GCD(ulong num1, ulong num2)
        {
            //uses Gabriel Lame's improvement to the Euclidean algorithm for greatest common divisor (GCD)
            if (num1 == 0 && num2 == 0) return 1;
            if (num1 == 0 || num2 == 0) return Math.Max(num1, num2);
            ulong temp;
            while (num1 > 0)
            {
                temp = num1;
                num1 = num2 % num1;
                num2 = temp;
            }
            return num2;
        }

        private static void DivideByGCD(ref ulong num1, ref ulong num2)
        {
            ulong gcd = GCD(num1, num2);
            num1 /= gcd;
            num2 /= gcd;
        }

        public void Reduce()
        {
            if (!IsReduced)
            {
                DivideByGCD(ref mNumerator, ref mDenominator);
                IsReduced = true;
            }

        }
        public Fraction GetReducedEquiv()
        {
            Fraction frac = this;
            frac.Reduce();
            return frac;
        }
        public void SetTo(ulong Numerator, ulong Denominator, int sign = 1, short exponent = 0)
        {
            mNumerator = Numerator;
            mDenominator = Denominator;
            mExponent = exponent;
            Sign = sign;
            if (Numerator == 0 || Denominator == 0) IsReduced = true; else IsReduced = false;
            IsExact = true;
        }
        public void SetToZero()
        {
            mNumerator = 0;
            mDenominator = 1;
            mExponent = 0;
            Sign = 1;
            IsReduced = true;
            IsExact = true;
        }
        public void Invert()
        {
            ulong temp = mNumerator;
            mNumerator = mDenominator;
            mDenominator = temp;
            if (mExponent == short.MinValue)
                //in the rare event that mExponent == short.MinValue, switching signs would cause overflow
                AvoidExponentOverFlow(ref this, -mExponent);
            else
                mExponent = (short)-mExponent;
        }
        private static ulong QuotientBits(ulong numerator, ulong denominator, out int exponent, out int sigBitIndex)
        {
            //returns the quotient numerator/denominator in binary as (whole number quotient) x 2^exponent
            //and the 0-based index of the most significant bit

            int sig_n = mostSigBitIndex(numerator); //index of the most significant 1 in the numerator
            int sig_d = mostSigBitIndex(denominator); //index of the most significant 1 in the denominator
            exponent = 0; //the number of places to move the "decimal" point

            if (denominator == 0) throw new DivideByZeroException();
            if (numerator == 0) { sigBitIndex = -1; return 0ul; }
            if (numerator == denominator) { sigBitIndex = 1; return 1ul; }
            if (denominator == 1) { sigBitIndex = sig_n; return numerator; }

            //left align most significant digits
            if (sig_n > sig_d) denominator <<= sig_n - sig_d; else numerator <<= sig_d - sig_n;

            ulong quotient = 0ul;
            sigBitIndex = 0; //the 0-based index of the most significant bit in quotient
            int leastOneBit = 0; //the 0-based index of the least significant 1 bit
            exponent = sig_n - sig_d;
            if (numerator < denominator)
            {
                //the first digit in quotient will be 0 which is not significant
                sigBitIndex--;
                exponent--;
            }
            ulong mostSigBit = 0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000ul;
            bool bitLoss = false; //set to true when 65 bits are necessary to left shift the numerator
            while (numerator > 0 && sigBitIndex < 64)
            {
                quotient <<= 1;
                if (numerator >= denominator)
                {
                    numerator -= denominator;
                    quotient |= 0b1;
                    leastOneBit = 0;
                }
                else if (bitLoss)
                {
                    //treat numerator (n) as if it were a 65 bit number as follows:
                    //2n is the same as left shifting n by 1 bit, making it effectively 65 bits
                    //let n = 2n - d = (n + n) - d = n + (n - d) = n - (d - n)
                    numerator -= denominator - numerator;
                    quotient |= 0b1;
                    leastOneBit = 0;
                }
                if ((numerator & mostSigBit) == 0) //left shifting will not result in loss of most significant bit
                {
                    numerator <<= 1;
                }
                else  //left shifting would result in loss of most significant bit
                {
                    bitLoss = true;
                }
                sigBitIndex++;
                leastOneBit++;
            }
            sigBitIndex--;
            leastOneBit--;
            exponent -= sigBitIndex;
            //remove trailing zeros (reduce the quotient) and increase the exponent accordingly
            quotient >>= leastOneBit;
            sigBitIndex -= leastOneBit;
            exponent += leastOneBit;
            return quotient;
        }
        private static ulong ReducedULong(ulong n, out int exponent)
        {
            exponent = 0;
            if (n == 0) return n;
            while ((n & 0b1ul) == 0)
            {
                n >>= 1;
                exponent++;
            }
            return n;
        }
        private static void FullyReduce(ref ulong numerator, ref ulong denominator, ref int exponent)
        {
            //used mainly for determining equality and ordering
            //the exponent may be out of short integer range
            DivideByGCD(ref numerator, ref denominator);
            numerator = ReducedULong(numerator, out int exp2);
            exponent += exp2;
            denominator = ReducedULong(denominator, out exp2);
            exponent -= exp2;
        }
        private static int mostSigBitIndex(ulong number)
        {
            //returns the index of the most significant ONE (1) bit
            if (number == 0) return -1;
            ulong selector = 0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000ul;
            int index = 63;
            while ((number & selector) == 0)
            {
                selector >>= 1;
                index--;
            }
            return index;
        }
        private static int leastSigBitIndex(ulong number)
        {
            //returns the index of the least significant ONE (1) bit
            if (number == 0) return -1;
            ulong selector = 0b1ul;
            int index = 0;
            while ((number & selector) == 0)
            {
                selector <<= 1;
                index++;
            }
            return index;
        }
        private static ulong OverflowProduct(ulong n1, ulong n2, out int exponent)
        {
            //returns the reduced (odd) product of n1 and n2 with an exponent
            //to be used when the regular product would cause overflow
            //seeks to find the highest possible precision in the product
            ulong n = 0;
            exponent = 0;
            bool overflow;
            do
            {
                overflow = false;
                try
                {
                    checked
                    {
                        n = n1 * n2;
                    }
                }
                catch (OverflowException)
                {
                    overflow = true;
                    ulong nn1 = ReducedULong(n1 >> 1, out int exp1);
                    ulong nn2 = ReducedULong(n2 >> 1, out int exp2);
                    if (mostSigBitIndex(nn1) > mostSigBitIndex(nn2))
                    {
                        n1 = nn1;
                        exponent += exp1;
                    }
                    else
                    {
                        n2 = nn2;
                        exponent += exp2;
                    }
                }
            } while (overflow);
            n = ReducedULong(n, out int exp);
            exponent += exp;
            return n;
        }
        private static void AvoidExponentOverFlow(ref Fraction result, int newExponent)
        {
            //makes sure the int exponent is in range of short.MinValue and short.MaxValue
            //and adjusts the numerator and denominator if necessary to avoid overflow/underflow
            //this routine favors keeping the numerator and denominator as small as possible
            //in order to avoid overflow in arithmetic operations

            if (newExponent > short.MaxValue)
            {
                result.Reduce();
                //need to reduce the exponent
                //to reduce the exponent, increase the overall fraction
                //method 1: decrease the denominator
                ulong temp = result.mDenominator;
                while (((temp & 0b1ul) == 0) & (newExponent > short.MaxValue))
                {
                    temp >>= 1;
                    newExponent--;
                }
                result.mDenominator = temp;

                //method 2: increase the numerator
                temp = result.mNumerator;
                while (((temp & 0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000) == 0) &
                    (newExponent > short.MaxValue))
                {
                    temp <<= 1;
                    newExponent--;
                }
                result.mNumerator = temp;

                if (newExponent > short.MaxValue)
                {
                    //nothing left to do without introducing error (less than 64 bits of accuracy in the quotient)
                    //return (+/-)infinity
                    result.SetTo(1, 0, result.Sign, 0);
                    result.IsReduced = true;
                    result.IsExact = false;
                    return;
                }
            }
            else if (newExponent < short.MinValue)
            {
                result.Reduce();
                //need to increase the exponent
                //to increase the exponent, decrease the overall fraction
                //method 1: decrease the numerator
                ulong temp = result.mNumerator;
                while (((temp & 0b1ul) == 0) & (newExponent < short.MinValue))
                {
                    temp >>= 1;
                    newExponent++;
                }
                result.mNumerator = temp;

                //method 2: increase the denominator
                temp = result.mDenominator;
                while (((temp & 0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000) == 0) &
                    (newExponent < short.MinValue))
                {
                    temp <<= 1;
                    newExponent++;
                }
                result.mDenominator = temp;

                if (newExponent < short.MaxValue)
                {
                    //nothing left to do without introducing error (less than 64 bits of accuracy in the quotient)
                    //return zero
                    result.mNumerator = 0;
                    result.mDenominator = 1;
                    result.Exponent = 0;
                    result.IsReduced = true;
                    result.IsExact = false;
                    return;
                }
            }
            result.mExponent = (short)newExponent;
        }

        private void PrintBits(ulong n)
        {
            for (int i = 63; i >= 0; i--)
            {
                if ((n & 0b1ul << i) == 0)
                {
                    Console.Write("0");
                }
                else
                {
                    Console.Write("1");
                }
                if (i % 4 == 0) Console.Write(" ");
            }
            Console.WriteLine(" ulong");
        }
        private void PrintBits(long n)
        {
            for (int i = 63; i >= 0; i--)
            {
                if ((n & 0b1L << i) == 0)
                {
                    Console.Write("0");
                }
                else
                {
                    Console.Write("1");
                }
                if (i % 4 == 0) Console.Write(" ");
            }
            Console.WriteLine(" long");
        }
        private void PrintBits(int n)
        {
            for (int i = 31; i >= 0; i--)
            {
                if ((n & 0b1 << i) == 0)
                {
                    Console.Write("0");
                }
                else
                {
                    Console.Write("1");
                }
                if (i % 4 == 0) Console.Write(" ");
            }
            Console.WriteLine(" int");
        }
        private static bool MultiplySpecialCases(Fraction a, Fraction b, ref Fraction result)
        {
            if (!a.IsFinite || !b.IsFinite) //one of the fractions is not finite
            {
                if (a.IsNaN || b.IsNaN) //one of the fractions is NaN
                {
                    //set result to NaN
                    result.SetTo(0, 0, 1, 0);
                }
                else if (a.mNumerator == 0 || b.mNumerator == 0) //one fraction is infinite and the other is zero
                {
                    //set result to NaN
                    result.SetTo(0, 0, 1, 0);
                }
                else
                {
                    //set result to (+/-)infinity)
                    result.SetTo(1, 0, a.Sign * b.Sign, 0);
                }
                return true;
            }
            else //both a and b are finite
            {
                if (a.mNumerator == 0 || b.mNumerator == 0)
                {
                    //set result to 0
                    result.SetToZero();
                    return true;
                }
            }
            return false;
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            ulong num, den;
            ulong a_num = a.mNumerator;
            ulong a_den = a.mDenominator;
            ulong b_num = b.mNumerator;
            ulong b_den = b.mDenominator;
            int exp = a.mExponent + b.mExponent;
            Fraction result = new Fraction()
            {
                mStates = (byte)(a.mStates & b.mStates), //set user flags to boolean product
                Sign = a.Sign * b.Sign,
                IsReduced = false, //not reduced is the default for this operation
                IsExact = a.IsExact && b.IsExact
            };

            if (MultiplySpecialCases(a, b, ref result)) return result;

            try
            {
                checked
                {
                    num = a_num * b_num;
                    den = a_den * b_den;
                }
            }
            catch (OverflowException)
            {
                //reduce each combination of numerator and denominator
                DivideByGCD(ref a_num, ref a_den);
                DivideByGCD(ref a_num, ref b_den);
                DivideByGCD(ref b_num, ref a_den);
                DivideByGCD(ref b_num, ref b_den);
                result.IsReduced = true;
                try
                {
                    checked
                    {
                        num = a_num * b_num;
                        den = a_den * b_den;
                    }
                }
                catch (OverflowException)
                {
                    //divide out factors of 2 and compensate with overall exponent
                    a_num = ReducedULong(a_num, out int exp2);
                    exp += exp2;
                    a_den = ReducedULong(a_den, out exp2);
                    exp -= exp2;
                    b_num = ReducedULong(b_num, out exp2);
                    exp += exp2;
                    b_den = ReducedULong(b_den, out exp2);
                    exp -= exp2;
                    try
                    {
                        checked
                        {
                            num = a_num * b_num;
                            den = a_den * b_den;
                        }
                    }
                    catch (OverflowException)
                    {
                        //everything has been reduced as much as possible
                        //we will no longer be able to maintain strict equality
                        //we will find the closest 64 bit approximation...
                        //start by pairing numerators and denominators differently to determine
                        //which combination gives us the greatest combined precision

                        result.IsReduced = true;
                        result.IsExact = false;

                        //first: the natural pairing
                        ulong q1_1 = QuotientBits(a_num, a_den, out int e1_1, out int b1_1);
                        ulong q1_2 = QuotientBits(b_num, b_den, out int e1_2, out int b1_2);

                        //second: switch the pairing
                        ulong q2_1 = QuotientBits(a_num, b_den, out int e2_1, out int b2_1);
                        ulong q2_2 = QuotientBits(b_num, a_den, out int e2_2, out int b2_2);

                        //denominator will be 1 and the product of the two fractions
                        //will be represented as follows: (sign)numerator/1 * 2^exponent
                        den = 1;
                        ulong num1 = OverflowProduct(q1_1, q1_2, out int exp_1); //the reduced approximate product of two numbers
                        ulong num2 = OverflowProduct(q2_1, q2_2, out int exp_2); //the reduced approximate product of two numbers
                        if (mostSigBitIndex(num1) > mostSigBitIndex(num2))
                        {
                            num = num1;
                            exp += e1_1 + e1_2 + exp_1;
                        }
                        else
                        {
                            num = num2;
                            exp += e2_1 + e2_2 + exp_2;
                        }
                    }
                }
            }
            result.mNumerator = num;
            result.mDenominator = den;
            AvoidExponentOverFlow(ref result, exp);
            return result;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            b.Invert();
            return a * b;
        }
        private static bool AddSpecialCases(Fraction a, Fraction b, ref Fraction result)
        {
            if (!a.IsFinite || !b.IsFinite) //one of the fractions is not finite
            {
                if (a.IsNaN || b.IsNaN) //one of the fractions is NaN
                {
                    //set result to NaN
                    result.SetTo(0, 0, 1, 0);
                }
                else //one of the fractions is infinite
                {
                    if (a.IsFinite)
                    {
                        result.SetTo(1, 0, b.Sign, 0);
                    }
                    else if (b.IsFinite)
                    {
                        result.SetTo(1, 0, a.Sign, 0);
                    }
                    else if (a.Sign == b.Sign)
                    {
                        //set result to (+/-)infinity
                        result.SetTo(1, 0, a.Sign, 0);
                    }
                    else
                    {
                        // -infinity + infinity
                        //set result to NaN
                        result.SetTo(0, 0, 1, 0);
                    }
                }
                return true;
            }
            if (a.mNumerator == 0)
            {
                result.SetTo(b.mNumerator, b.mDenominator, b.Sign, b.mExponent);
                return true;
            }
            if (b.mNumerator == 0)
            {
                result.SetTo(a.mNumerator, a.mDenominator, a.Sign, a.mExponent);
                return true;
            }
            return false;
        }
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return Add(a, b, NumRecursiveCalls: 0); //start off with 0 recursive calls
            //under rare circumstancs, the Add function could result in infinite recursion
            //hence, the number of recursive calls is counted in order to force an answer after a finite number of calls
        }
        private static Fraction Add(Fraction a, Fraction b, int NumRecursiveCalls)
        {
            //Consider: a + b = [(sign1)n1/d1 x 2^exp1] + [(sign2)n2/d2 x 2^exp2].
            //If exp1 == exp2 then a + b = [(sign1)*n1*d2 + (sign2)n2*d1]/[d1*d2]*2^exp where exp = exp1 = exp2.
            //Since we're storing each numerator and denominator as usigned longs, it's also very important to consider
            //the signs of each fraction. Example: 5 + (-7) = -2, which must be processed as -(7-5).

            //However, if exp2 != exp2 then the relative sizes of the two fractions could be drastically different.
            //Example: consider decimal numbers as a special case of fractions where the denominators are always powers of ten...
            //2453 + 4.603 = 2457.603, but supposing we could only store four digits, this would be
            //rounded to 2458 or truncated to 2457. Furthermore, if the magnitudes (exponents) are very different,
            //then the sum would be truncated to the largest (most significant) fraction in magnitude.

            Fraction result = new Fraction()
            {
                mStates = (byte)(a.mStates | b.mStates), //set user flags to boolean sum
                IsReduced = false, //not reduced is the default for this operation
                IsExact = a.IsExact && b.IsExact
                //Sign to be determined later
            };

            if (AddSpecialCases(a, b, ref result)) return result;

            int sign;

            if (a.mExponent == b.mExponent)
            {
                ulong num, num1, num2, den; //num1 is the first part of the numerator, num2 the second
                ulong a_num = a.mNumerator;
                ulong a_den = a.mDenominator;
                ulong b_num = b.mNumerator;
                ulong b_den = b.mDenominator;
                try
                {
                    checked
                    {
                        //get common denominators
                        num1 = a_num * b_den;
                        num2 = b_num * a_den;
                        den = a_den * b_den;
                    }
                }
                catch (OverflowException)
                {
                    //reduce each pair of numerator and denominator
                    DivideByGCD(ref a_num, ref a_den);
                    DivideByGCD(ref b_num, ref b_den);
                    try
                    {
                        checked
                        {
                            num1 = a_num * b_den;
                            num2 = b_num * a_den;
                            den = a_den * b_den;
                        }
                    }
                    catch (OverflowException)
                    {
                        //Getting common denominators is not possible so to add these fractions,
                        //we try modifying exponents and call this operator recurisively
                        //divide out factors of 2 and compensate with overall exponent
                        int a_exp = a.mExponent;
                        int b_exp = b.mExponent;
                        a.mNumerator = ReducedULong(a_num, out int exp2);
                        a_exp += exp2;
                        a.mDenominator = ReducedULong(a_den, out exp2);
                        a_exp -= exp2;
                        b.mNumerator = ReducedULong(b_num, out exp2);
                        b_exp += exp2;
                        b.mDenominator = ReducedULong(b_den, out exp2);
                        b_exp -= exp2;
                        AvoidExponentOverFlow(ref a, a_exp);
                        AvoidExponentOverFlow(ref b, b_exp);
                        if (!a.IsFinite || !b.IsFinite) // (+/-)infinity or NaN
                        {
                            //do binary approximation instead
                            return ApproxSum(a, b);
                        }
                        else
                        {
                            if (NumRecursiveCalls < 3)
                            {
                                return Add(a, b, NumRecursiveCalls++); //recursive call
                            }
                            else
                            {
                                return ApproxSum(a, b);
                            }
                        }
                    }
                }
                //Now try combining fractions
                try
                {
                    checked
                    {
                        if (a.Sign == b.Sign)
                        {
                            num = num1 + num2;
                            sign = a.Sign;
                        }
                        else
                        {
                            //one must be subtracted from the other, must know which is the largest in absolute value
                            if (a.Sign > 0) //a positive (num1), b negative (num2)
                            {
                                if (num1 >= num2) //sum is positive
                                {
                                    num = num1 - num2;
                                    sign = 1;
                                }
                                else //sum is negative
                                {
                                    num = num2 - num1;
                                    sign = -1;
                                }
                            }
                            else //a negative (num1), b positive (num2)
                            {
                                if (num1 > num2) //sum is negative
                                {
                                    num = num1 - num2;
                                    sign = -1;
                                }
                                else //sum is negative
                                {
                                    num = num2 - num1;
                                    sign = 1;
                                }
                            }
                        }
                    }
                    //**********************************************************************************
                    //at this point, we have managed to avoid overflow and keep exponents equal. YAY!!!
                    //**********************************************************************************
                    result.SetTo(num, den, sign, a.mExponent);
                    return result;
                }
                catch (OverflowException)
                {
                    //Getting common denominators is not possible so to add these fractions,
                    //we try modifying exponents and call this operator recurisively
                    //divide out factors of 2 and compensate with overall exponent
                    int a_exp = a.mExponent;
                    int b_exp = b.mExponent;
                    a.mNumerator = ReducedULong(a_num, out int exp2);
                    a_exp += exp2;
                    a.mDenominator = ReducedULong(a_den, out exp2);
                    a_exp -= exp2;
                    b.mNumerator = ReducedULong(b_num, out exp2);
                    b_exp += exp2;
                    b.mDenominator = ReducedULong(b_den, out exp2);
                    b_exp -= exp2;
                    AvoidExponentOverFlow(ref a, a_exp);
                    AvoidExponentOverFlow(ref b, b_exp);
                    if (!a.IsFinite || !b.IsFinite) // (+/-)infinity or NaN
                    {
                        //do binary approximation instead
                        return ApproxSum(a, b);
                    }
                    else
                    {
                        if (NumRecursiveCalls < 3)
                        {
                            return Add(a, b, NumRecursiveCalls++); //recursive call
                        }
                        else
                        {
                            return ApproxSum(a, b);
                        }
                    }
                }
            }
            else //the exponents are not equal
            {
                //try making the exponents equal
                if (NumRecursiveCalls > 2) return ApproxSum(a, b);
                Fraction fracSmallerExp, fracLargerExp;
                if (a.mExponent < b.mExponent)
                {
                    fracSmallerExp = a;
                    fracLargerExp = b;
                }
                else
                {
                    fracSmallerExp = b;
                    fracLargerExp = a;
                }
                int numSmallerExpLeastBit = leastSigBitIndex(fracSmallerExp.mNumerator);
                int denSmallerExpMostBit = mostSigBitIndex(fracSmallerExp.mDenominator);
                int numLargerExpMostBit = mostSigBitIndex(fracLargerExp.mNumerator);
                int denLargerExpLeastBit = leastSigBitIndex(fracLargerExp.mDenominator);
                int smallerExpMax = fracSmallerExp.mExponent + numSmallerExpLeastBit + (63 - denSmallerExpMostBit);
                int largerExpMin = fracLargerExp.mExponent - denLargerExpLeastBit - (63 - numLargerExpMostBit);
                if (smallerExpMax < largerExpMin) //the exponents CANNOT be made the same
                {
                    return ApproxSum(a, b);
                }
                else //the exponents CAN be made the same
                {
                    //exponent overflow/underflow is not an issue since the exponents will only be brought to an intermediate value
                    int expDiff = fracLargerExp.mExponent - fracSmallerExp.mExponent;
                    int reduceEffect = numSmallerExpLeastBit + denLargerExpLeastBit;
                    //start by making numbers smaller (try to avoid overflow on the recursive call)
                    if (reduceEffect <= expDiff)
                    {
                        //apply full effect, the full effect is either not sufficient or it is barely sufficient
                        fracSmallerExp.mNumerator = ReducedULong(fracSmallerExp.mNumerator, out int exp);
                        fracSmallerExp.mExponent += (short)exp;
                        fracLargerExp.mDenominator = ReducedULong(fracLargerExp.mDenominator, out exp);
                        fracLargerExp.mExponent -= (short)exp;
                    }
                    else
                    {
                        //apply partial effect, a partial effect is sufficient to make exponents equal
                        do
                        {
                            if (numSmallerExpLeastBit > denLargerExpLeastBit)
                            {
                                fracSmallerExp.mNumerator >>= 1;
                                fracSmallerExp.mExponent++;
                            }
                            else
                            {
                                fracLargerExp.mDenominator >>= 1;
                                fracLargerExp.mExponent--;
                            }
                        } while (fracSmallerExp.mExponent != fracLargerExp.mExponent);
                        return Add(fracSmallerExp, fracLargerExp, NumRecursiveCalls++);
                    }
                    //now make numbers larger
                    while (fracSmallerExp.mExponent != fracLargerExp.mExponent) //
                    {
                        if (denSmallerExpMostBit < numLargerExpMostBit)
                        {
                            fracSmallerExp.mDenominator <<= 1;
                            fracSmallerExp.mExponent++;
                        }
                        else
                        {
                            fracLargerExp.mNumerator <<= 1;
                            fracLargerExp.mExponent--;
                        }
                    }
                    //exponents are now equal
                    return Add(fracSmallerExp, fracLargerExp, NumRecursiveCalls++);
                }
            }
        }
        private static Fraction ApproxSum(Fraction a, Fraction b)
        {
            Fraction result = new Fraction()
            {
                mStates = (byte)(a.mStates | b.mStates),
                IsReduced = true,
                IsExact = false
            };
            if (AddSpecialCases(a, b, ref result)) return result;
            ulong q1 = QuotientBits(a.Numerator, a.Denominator, out int exp1, out int sigbit1);
            ulong q2 = QuotientBits(b.Numerator, b.Denominator, out int exp2, out int sigbit2);
            exp1 += a.mExponent;
            exp2 += b.mExponent;
            int exp = 0; //exponent of the sum
            int b1_most, b1_least; //the place values of the most and least significant bits, respectively, of q1
            int b2_most, b2_least; //the place values of the most and least significant bits, respectively, of q2
            b1_least = exp1;
            b1_most = exp1 + sigbit1;
            b2_least = exp2;
            b2_most = exp2 + sigbit2;

            if (b1_least > b2_most || b2_least > b1_most) //the two quotients DO NOT have any digits of equal place value in common
            {
                //q1: ----------------
                //q2:                    ----------------
                //                    OR
                //q1:                    ----------------
                //q2: ----------------

                //return the largest ranking fraction (with most significant digits)
                if (b1_most > b2_most)
                {
                    a.IsExact = false;
                    return a;
                }
                else
                {
                    b.IsExact = false;
                    return b;
                }
            }
            else //the two quotients DO have digits of equal place value in common
            {
                //expChange is kept in separate variable so that we don't have to check overflow/underflow numerous times
                int shift;
                int shiftMax;
                //align the digis of equal place value
                // there are four ways the two quotients can overlap:
                if (b1_most >= b2_most && b1_least <= b2_least)
                {
                    //q1: ----------------
                    //q2:   ------------
                    exp = exp1;
                    shift = exp2 - exp1;
                    q2 <<= shift; //fill in lower rank zeros
                }
                else if (b2_most >= b1_most && b2_least <= b1_least)
                {
                    //q1:   ------------
                    //q2: ----------------
                    exp = exp2;
                    shift = exp1 - exp2;
                    q1 <<= shift; //fill in lower rank zeros
                }
                else if (b1_most >= b2_most && b1_least >= b2_least)
                {
                    //q1: ----------------
                    //q2:     ----------------
                    exp = exp2;
                    shift = exp1 - exp2;
                    shiftMax = 64 - (b1_most - b1_least + 1); //64 - (number of digits in q1)
                    if (shift > shiftMax)
                    {
                        q1 <<= shiftMax; //fill in lower rank zeros
                        shift -= shiftMax;
                        q2 >>= shift; //discard lower rank digits
                        exp += shift;
                    }
                    else
                    {
                        q1 <<= shift;
                    }
                }
                else //if (b2_most >= b1_most && b2_least >= b1_least)
                {
                    //q1:     ----------------
                    //q2: ----------------
                    exp = exp1;
                    shift = exp2 - exp1;
                    shiftMax = 64 - (b2_most - b2_least + 1); //64 - (number of digits in q2)
                    if (shift > shiftMax)
                    {
                        q2 <<= shiftMax; //fill in lower rank zeros
                        exp -= shiftMax;
                        shift -= shiftMax;
                        q1 >>= shift; //discard lower rank digits
                        exp += shift;
                    }
                    else
                    {
                        q2 <<= shift;
                    }
                }
            }

            //Account for the signs of each fraction
            int s1 = a.Sign;
            int s2 = b.Sign;
            int sign = 0; //sign of the sum
            ulong num;
            if (s1 == s2)
            {
                sign = s1;
                try
                {
                    checked
                    {
                        num = q1 + q2;
                    }
                }
                catch (OverflowException)
                {
                    //discard least significant digits
                    q1 >>= 1;
                    q2 >>= 1;
                    exp++;
                    num = q1 + q2; //no overflow since a 63 digit num + 63 digit num = 64 digit num
                }
            }
            else //signs are opposite
            {
                if (q1 > q2)
                {
                    num = q1 - q2;
                    if (s1 < 0) sign = -1; else sign = 1;
                }
                else
                {
                    num = q2 - q1;
                    if (s1 < 0) sign = 1; else sign = -1;
                }
            }
            result.mNumerator = num;
            result.mDenominator = 1;
            result.Sign = sign;
            result.mExponent = 0;
            AvoidExponentOverFlow(ref result, exp);
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Fraction fraction && fraction == this;
        }
        public bool ApproximatelyEquals(Fraction otherFraction)
        {
            //uses 64 bit quotient approximation to determine if two fractions are appoximately equal
            //As a decimal example: 10,000/30,001 = 0.3333444448 is approximately equal to 1/3 = 0.3333333333 in the range of 4 digits
            //Equality of exponents is evaluated in the range of int, so if the conversion to an
            //approximation causes exponent overload/underload in the range of short, this is ignored.
            //Hence very small or very large fractions (in absolute value) may not be considered equal,
            //even though they might have been considered equal had they undergone a full conversion,
            //possibly to zero or (+/-)infinity.
            if (mNumerator == 0 || mDenominator == 0 || otherFraction.mNumerator == 0 || otherFraction.mDenominator == 0)
            {
                //aproximation is not necessary with 0, (+/-)infinity, or NaN
                return this == otherFraction;
            }
            ulong a_q = QuotientBits(mNumerator, mDenominator, out int a_exp, out int a_sb);
            a_exp += this.mExponent;
            ulong b_q = QuotientBits(otherFraction.mNumerator, otherFraction.mDenominator, out int b_exp, out int b_sb);
            b_exp += otherFraction.mExponent;
            if (a_q == b_q && a_exp == b_exp) return true; else return false;
        }
        public override int GetHashCode()
        {
            //the following method ensures that two fractions which are numerically equal
            //will have the same hashcode, even if their numerators, denominators, or exponents are different
            ulong num = mNumerator;
            ulong den = mDenominator;
            int exp = mExponent;
            FullyReduce(ref num, ref den, ref exp);
            return HashCode.Combine(num, den, exp, Sign);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            b.Sign = -b.Sign;
            return a + b;
        }
        public static bool operator ==(Fraction a, Fraction b)
        {
            //Only returns true if a and b are exactly equal in the following sense:
            //If a == (sign1)n1/d1 x 2^exp1 and b == (sign2)n2/d2 x 2^exp2 and both fractions are fully reduced then
            //sign1 == sign2, n1 == n2, d1 == d2, and exp1 == exp2
            //This method is chosen over the n1*d2 == n2*d1 method, which is faster, but may cause overflow.
            //This routine ignores whether or not both fractions are in a state of "exactness"
            if (a.Sign != b.Sign) return false;
            if (a.mDenominator == 0 && b.mDenominator == 0)
            {
                if (a.mNumerator == b.mNumerator) return true; else return false;
            }
            if (a.mDenominator == 0 || b.mDenominator == 0) return false;
            if (a.mNumerator == 0 && b.mNumerator == 0) return true;

            if (a.mNumerator == b.mNumerator &&
                a.mDenominator == b.mDenominator &&
                a.mExponent == b.mExponent)
                return true;
            else
            {
                int a_exp = a.mExponent;
                int b_exp = b.mExponent;
                FullyReduce(ref a.mNumerator, ref a.mDenominator, ref a_exp);
                FullyReduce(ref b.mNumerator, ref b.mDenominator, ref b_exp);
                if (a.mNumerator == b.mNumerator &&
                    a.mDenominator == b.mDenominator &&
                    a_exp == b_exp)
                    return true;
                else
                    return false;
            }
        }
        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }
        public static bool operator <(Fraction a, Fraction b)
        {
            //we attempt to first resolve the inequality using exact comparisons
            //if this causes overflow, we switch to an approximate comparison, which may not be strictly correct
            //for instance, it's possible (but unlikely) for two different fractions to have the same 64 bit approximation
            if (a.mDenominator == 0 || b.mDenominator == 0)
            {
                //return true when -inf < +inf
                //retun false when -inf < -inf, +inf < +inf, or whenever a or b is NaN
                if (a.IsNegInfinity && b.IsPosInfinity) return true; else return false;
            }
            //if did not return, then both fractions are finite
            int a_sign = a.Sign;
            int b_sign = b.Sign;
            if (a_sign == b_sign) //same sign
            {
                if (a_sign < 0) return b < a; //note the equivalence of -x < -y and y < x
                //if did not return, then both fractions are positive
                if (a.mNumerator == 0 && b.mNumerator == 0) return false; //both are zero
                if (a.mNumerator == 0) return true;
                if (b.mNumerator == 0) return false;
                int a_exp = a.mExponent;
                int b_exp = b.mExponent;
                if (a_exp == b_exp)
                {
                    try
                    {
                        return a.mNumerator * b.mDenominator < b.mNumerator * a.mDenominator;
                    }
                    catch (OverflowException)
                    {
                        FullyReduce(ref a.mNumerator, ref a.mDenominator, ref a_exp);
                        FullyReduce(ref b.mNumerator, ref b.mDenominator, ref b_exp);
                        try
                        {
                            return a.mNumerator * b.mDenominator < b.mNumerator * a.mDenominator;
                        }
                        catch (OverflowException)
                        {
                            //do nothing
                            //resolve below with approximations
                        }
                    }
                }
                //exponents are not equal or exact comparison caused overflow
                //try rough (and fast) comparison first
                if (a_exp + 63 < b_exp) return true; //the exponent of a is too small for a to possibly be the larger number
                if (a_exp > b_exp + 63) return false; //the exponent of a is too large for a to possibly be the smaller number
                //perform approximate comparison
                ulong a_q = QuotientBits(a.mNumerator, a.mDenominator, out int a_exp2, out int a_sigBit);
                a_exp += a_exp2;
                ulong b_q = QuotientBits(a.mNumerator, a.mDenominator, out int b_exp2, out int b_sigBit);
                b_exp += b_exp2;
                if (a_sigBit + a_exp < b_sigBit + b_exp) return true; //a's most significant bit has lower place value than b's
                if (a_sigBit + a_exp > b_sigBit + b_exp) return false; //a's most significant bit has higher place value than b's
                //if did not return, both a's and b's most signicant bits have the same place value
                //we must perform a bitwise comparison
                a_q <<= 63 - a_sigBit; //transform to 64 bit number
                b_q <<= 63 - b_sigBit; //transform to 64 bit number
                ulong selector = 0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                do
                {
                    selector >>= 1;
                    if ((a_q & selector) == 0 && (b_q & selector) != 0) return true;
                    if ((a_q & selector) != 0 && (b_q & selector) == 0) return false;
                } while (selector > 0);
                return false; //both numbers have the same approximation
            }
            else if (a_sign < 0) //different signs
                return true;
            else
                return false;
        }
        public static bool operator <=(Fraction a, Fraction b)
        {
            return (a < b) || (a == b);
        }
        public static bool operator >(Fraction a, Fraction b)
        {
            return !(a <= b);
        }
        public static bool operator >=(Fraction a, Fraction b)
        {
            return !(a < b);
        }
        public static implicit operator Fraction(ulong number)
        {
            //implicitly casts ulong to fraction
            return new Fraction
            {
                mNumerator = number,
                mDenominator = 1,
                mExponent = 0,
                Sign = 1,
                IsReduced = true,
                IsExact = true
            };
        }
        public static implicit operator Fraction(long number)
        {
            //implicitly casts long to fraction
            if (number < 0)
                return new Fraction
                {
                    mNumerator = (ulong)(-number),
                    mDenominator = 1,
                    mExponent = 0,
                    Sign = -1,
                    IsReduced = true,
                    IsExact = true
                };
            else
                return new Fraction
                {
                    mNumerator = (ulong)(number),
                    mDenominator = 1,
                    mExponent = 0,
                    Sign = 1,
                    IsReduced = true,
                    IsExact = true
                };
        }
        public static implicit operator Fraction(double number)
        {
            //implicitly casts double to fraction
            return new Fraction { DoubleValue = number };
        }
        public override string ToString()
        {
            if (mDenominator == 0)
            {
                if (mNumerator == 0) return "NaN";
                if (Sign > 0) return "infinity"; else return "-infinity";
            }
            string result;
            if (Sign < 0) result = "-"; else result = "";
            result += mNumerator.ToString();
            if (mDenominator != 1) result += "/" + mDenominator.ToString();
            if (mExponent != 0) result += " x 2^" + mExponent.ToString();
            return result;
        }
    }
}
