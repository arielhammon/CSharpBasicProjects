using System;
using System.Collections.Generic;
using System.Text;

namespace ClassMethodsOverloading
{
    class PrimeOps
    {
        private ulong[] sieve;  //represents sieve of Eratosthenes for calculating primes, managed bitwise to save memory
        private bool sieveComputed;
        private ulong mNumber;
        private ulong mSieveLength;
        private const ulong ticker = int.MaxValue / 64;
        
        public ulong Number()
        {
            return mNumber;
        }
        public ulong SieveLength()
        {
            return mSieveLength;
        }

        public PrimeOps()
        {
            Initialize(ulong.MaxValue);
        }

        public PrimeOps(ulong Number)
        {
            Initialize(Number);
        }
        private void Initialize(ulong Number)
        {
            this.mNumber = Number;
            ulong n = MaxSmallestPrimeFactor(Number);
            n += (n % 128 == 0 ? 0 : 128 - (n % 128)); //increase n to multiple of 128; each 64 bit ulong representing 64 odds, a span of 128 numbers
            n /= 128;
            mSieveLength = n;
            sieve = new ulong[mSieveLength];

            ulong tickerCt = 0;
            
            //initialize sieve with bitwise true values representing 1, 3, 5, 7, 9, etc., evens removed since 2 is the only even prime
            ulong odds;
            odds = 0b1111111111111111111111111111111111111111111111111111111111111111; //0-based index starts at 1, represents 1, 3, ..., 125, 127
            for (ulong i = 0; i < n; i++)
            {
                sieve[i] = odds;
                tickerCt++;
                if (tickerCt % ticker == 0) Console.Write(".");
            }
            Console.WriteLine("Sieve of Eratosthenes initialized.");
            sieveComputed = false; //initialized but not computed
        }
        public ulong MaxSmallestPrimeFactor(ulong num)
        {
            ulong nmaxfloor = Convert.ToUInt64(Math.Sqrt(Convert.ToDouble(num))); //the approx. maximum of the smallest prime factor
            //in case of num close to ulong.MaxValue converting ulong to double may result in loss of precision and rounding errors
            //so below we're searching for the exact floor of the square root
            //each of the upperbound/lowerbound loops execute at most twice
            //start by finding an upperbound for the floor of the square root
            int iterations = 0; //used for diagnostic purposes
            bool upperBoundFound = false;
            do
            {
                try
                {
                    checked
                    {
                        if (nmaxfloor * nmaxfloor < num)
                        {
                            nmaxfloor++;
                        }
                        else
                        {
                            upperBoundFound = true;
                        }
                    }
                }
                catch (OverflowException ex)
                {
                    upperBoundFound = true; //if overflow occurs, we've found an upperbound
                    Console.WriteLine("Overflow occured in search for ceiling of max least prime. No worries. We recovered.");
                }
                iterations++;
            } while (!upperBoundFound);
            Console.WriteLine("Upperbound search looped {0} times.", iterations);
            iterations = 0;
            //now find a lowerbound for the floor of the square root
            bool lowerBoundFound = false;
            do
            {
                try
                {
                    checked
                    {
                        if (nmaxfloor * nmaxfloor > num)
                        {
                            nmaxfloor--;
                        }
                        else
                        {
                            lowerBoundFound = true;
                        }
                    }
                }
                catch (OverflowException ex)
                {
                    nmaxfloor--;
                    lowerBoundFound = false; //if overflow occurs, we haven't found a lowerbound
                    Console.WriteLine("Overflow occured in search for floor of max least prime. No worries. We recovered.");
                }
                iterations++;
            } while (!lowerBoundFound);
            Console.WriteLine("Lowerbound search looped {0} times.", iterations);
            Console.WriteLine("Successfully found an upperbound for the smallest prime factor.");

            return nmaxfloor;
        }

        public void ComputeSieve()
        {
            ulong endNum = mSieveLength*128 - 1; //the number represented by the last bit in the array
            ulong nn = Convert.ToUInt64(Math.Sqrt(Convert.ToDouble(endNum))); //the max smallest prime needed to calculate the sieve
            //the above conversions experience no loss of precision in the range of endNum (approx 32 bit)

            ulong tickerCt = 0;

            ulong n, m; //the prime in the sieve and it's multiples
            ulong ni; //outer index of prime crawler/search (the ulong being inspected)
            ulong nj; //inner index of prime crawler/search (the bit being inspect)
            ulong i; //outer index
            int j; //inner index
            ulong jselector, nselector;
            
            nselector = 0b0100000000000000000000000000000000000000000000000000000000000000; //initialized to 3
            ni = 0; //the first ulong
            nj = 1; //initialized to 3, evens excluded
            n = 128 * ni + 2 * nj + 1;
            while (n <= nn) //the number represented by the current bit being inspected is <= to the max smallest prime
            {
                if ((sieve[ni] & nselector) == nselector) //found prime, so remove it's multiples
                {
                    m = n + 2*n; //only odd multiples of n
                    while (m <= endNum)
                    {
                        //eliminate m from the sieve
                        i = m / 128; //the primary index (which ulong to modify), divided by 128 since each ulong represents a span of 128 numbers
                        j = ((int)m % 128) / 2; //the inner index (which bit to modiy), divided by 2 since evens excluded
                        jselector = 0b1000000000000000000000000000000000000000000000000000000000000000 >> j;
                        //used to select a single bit, with the 1 bit shifted right to index j
                        sieve[i] = sieve[i] & ~jselector; //keep all bits the same except turn off the bit at index j
                        m += 2*n; //take the next odd multiple of n
                        tickerCt++;
                        if (tickerCt % ticker == 0) Console.Write(".");
                    }
                }
                //advance, look for next prime
                nj++;
                if (nj == 128)
                {
                    ni++; //advance to next ulong
                    nj = 0; //set to first bit
                    nselector = 0b1000000000000000000000000000000000000000000000000000000000000000; //set to first bit
                }
                else
                {
                    nselector >>= 1; //move selector to next bit
                }
                tickerCt++;
                if (tickerCt % ticker == 0) Console.Write(".");
                n = 128 * ni + 2 * nj + 1;
            }
            Console.WriteLine("Sieve of Eratosthenes fully computed.");
        }

        public ulong SmallestPrimeFactor(ulong num)
        {
            if (num > mNumber) throw new ArgumentOutOfRangeException("num", "The parameter must be between 0 and the initializing number, inclusive.");
            if (num % 2 == 0) return 2; //the number is divisible by 2 which is prime
            if (!sieveComputed)
            {
                ComputeSieve();
                sieveComputed = true;
            }
            ulong nn = Convert.ToUInt64(Math.Sqrt(Convert.ToDouble(num))); //the max smallest prime factor of num
            //the above conversions experience no loss of precision in the range of num (approx 32 bit)

            ulong tickerCt = 0;

            ulong n; //the prime in the sieve
            ulong ni; //outer index of prime crawler/search (the ulong being inspected)
            ulong nj; //inner index of prime crawler/search (the bit being inspect)
            ulong nselector;

            nselector = 0b0100000000000000000000000000000000000000000000000000000000000000; //initialized to 3
            ni = 0; //the first ulong
            nj = 1; //initialized to 3, evens excluded
            n = 128 * ni + 2 * nj + 1;
            while (n <= nn) //the number represented by the current bit being inspected is <= max smallest prime factor
            {
                if ((sieve[ni] & nselector) == nselector) //found prime
                {
                    if (num % n == 0) return n; //the number is divisible by n which is prime
                }
                //advance, look for next prime
                nj++;
                if (nj == 128)
                {
                    ni++; //advance to next ulong
                    nj = 0; //set to first bit
                    nselector = 0b1000000000000000000000000000000000000000000000000000000000000000; //set to first bit
                }
                else
                {
                    nselector >>= 1; //move selector to next bit
                }
                tickerCt++;
                if (tickerCt % ticker == 0) Console.Write(".");
                n = 128 * ni + 2 * nj + 1;
            }
            return 1; //if the smallest prime factor is 1, then the initializing number was itself prime
        }

        public ulong SmallestPrimeFactor()
        {
            return SmallestPrimeFactor(mNumber);
        }

        public ulong SmallestPrimeFactor(double num)
        {
            num = Math.Floor(num);
            if (num > Convert.ToDouble(mNumber)) throw new ArgumentOutOfRangeException("num", "The parameter must be between 0 and the initializing number, inclusive.");
            ulong ulongNum = Convert.ToUInt64(num);
            return SmallestPrimeFactor(ulongNum);
        }

        public ulong SmallestPrimeFactor(string num)
        {
            ulong ulongNum;
            if (!ulong.TryParse(num,out ulongNum)) throw new FormatException("The string could not be converted to an unsigned 64 bit integer.");
            return SmallestPrimeFactor(ulongNum);
        }
    }
}
