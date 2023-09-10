using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Parser.ShadowRun
{
    public class MikeHelper
    {
        private static Random random = new Random();

        public static long GetCurrentTime()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string ComputeSHA256(string s)
        {
            string hash = String.Empty;

            // Initialize a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the given string
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

                // Convert the byte array to string format
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }

            return hash.ToLower();
        }
    }
}
