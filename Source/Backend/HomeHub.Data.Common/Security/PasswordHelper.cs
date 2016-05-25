using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HomeHub.Data.Common.Security
{
    using HomeHub.Common.Exceptions;

    public static class PasswordHelper
    {
        /// <summary>
        /// Gets a byte array full of random
        /// </summary>
        /// <param name="length">The length of the byte array</param>
        /// <returns>A byte array full of some random</returns>
        public static byte[] GenerateRandom(int length)
        {
            var output = new byte[length];

            using (var randy = new RNGCryptoServiceProvider())
            {
                randy.GetBytes(output);
            }

            return output;
        }

        /// <summary>
        /// Hashes a password with a salt
        /// </summary>
        /// <param name="salt">The salt</param>
        /// <param name="password">The password</param>
        /// <returns>The hashed password</returns>
        public static byte[] HashPassword(byte[] salt, byte[] password)
        {
            var fullbytes = new byte[salt.Length + password.Length];

            var ind = 0;
            foreach (var t in salt)
            {
                fullbytes[ind++] = t;
            }
            foreach (var t in password)
            {
                fullbytes[ind++] = t;
            }

            using (var sha = SHA512.Create())
            {
                var res = sha.ComputeHash(fullbytes);

                // run some rounds
                for (var i = 0; i < 36; i++)
                {
                    res = sha.ComputeHash(res);
                }

                return res;
            }
        }

        /// <summary>
        /// Check to see if a string password matches the hash
        /// </summary>
        /// <param name="salt">The salt</param>
        /// <param name="hashedPass">The hashed password</param>
        /// <param name="testPass">The string password, encoded in unicode</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool ArePasswordsEqual(byte[] salt, byte[] hashedPass, string testPass)
        {
            var encodedPass = Encoding.Unicode.GetBytes(testPass);

            var hashedTestPass = PasswordHelper.HashPassword(salt, encodedPass);

            if (hashedTestPass.Length != hashedPass.Length)
            {
                return false;
            }

            for (var i = 0; i < hashedPass.Length; i++)
            {
                if (hashedPass[i] != hashedTestPass[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the bytes from an ip address
        /// </summary>
        /// <param name="ip">The ip address</param>
        /// <returns>16 bytes</returns>
        public static byte[] GetIPAddressInSqlForm(IPAddress ip)
        {
            var ipv6 = ip.AddressFamily != AddressFamily.InterNetworkV6 ? ip.MapToIPv6() : ip;
            return ipv6.GetAddressBytes();
        }
    }
}
