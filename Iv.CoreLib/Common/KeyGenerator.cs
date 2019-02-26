using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace Iv.Common
{
    public class KeyGenerator
    {
        
        private static int SeedCounter = 0;
        private readonly object SeedInitLock = new Object();

        public static int GetNumber(int min = 0, int max = 0)
        {
            Random random = GetRandom();
            return random.Next(min, max);
        }
        
        public static string GetString(int size = 6, bool lowerCase = false)
        {
            StringBuilder builder = new StringBuilder();
            Random random = GetRandom();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string GetUniqueKey(int maxSize)
        {

            char[] chars = new char[63];
            string a = null;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[2];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public static int GetRandomNumber(int min = 0, int max = Int32.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        private static Random GetRandom()
        {
            //Do init the first time this function is ever called.
            if (SeedCounter == -1)
            {
                //The first time the function is called everyone will try to update SeedCounter, but only the first 
                //thread to complete it will be the value everyone uses.
                Random initRNG = new Random();
                Interlocked.CompareExchange(ref SeedCounter, initRNG.Next(), -1);

            }
            else if (SeedCounter < 0)
            {
                //Because Interlocked.Increment wraps the value to int.MinValue and Random(int) will take the absolute
                //value of the seed, we skip all of the negitive numbers and go to 0.
                Interlocked.CompareExchange(ref SeedCounter, 0, int.MinValue);
            }

            int tempSeed = Interlocked.Increment(ref SeedCounter);
            if (tempSeed < 0)
            {
                //If tempSeed is negative we hit a edge case where SeedCounter wrapped around. We just call the function
                //again so we do not reuse a seed that was just used.
                return GetRandom();
            }

            return new Random(tempSeed);
        }
    }
}
