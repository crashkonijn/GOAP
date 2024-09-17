using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CrashKonijn.Goap.Runtime
{
    public class GuidCacheKey
    {
        /// <summary>
        /// Generates a unique, order-independent key from a list of Guids.
        /// No LINQ is used, and minimal garbage collection occurs during execution.
        /// </summary>
        /// <param name="guids">The list of Guids.</param>
        /// <returns>A unique hash string based on the sorted Guids.</returns>
        public static string GenerateKey(List<Guid> guids)
        {
            if (guids == null || guids.Count == 0)
                throw new ArgumentException("Guid list must not be null or empty.");

            // Sort the List in-place to ensure order independence
            guids.Sort(CompareGuids);

            // Use StringBuilder to concatenate Guids into a single string
            // Pre-allocate enough space to avoid resizing (guid.Length * 36 -> Guid is 36 characters when stringified)
            var stringBuilder = new StringBuilder(guids.Count * 36);

            foreach (var guid in guids)
            {
                // Append each Guid as a string without allocating new memory each time
                stringBuilder.Append(guid.ToString("N")); // "N" format avoids hyphens and produces 32-character strings
            }

            // Get the final concatenated string of sorted Guids
            var concatenatedGuids = stringBuilder.ToString();

            // Return the hashed result (SHA-256)
            return ComputeSha256Hash(concatenatedGuids);
        }

        /// <summary>
        /// Computes the SHA-256 hash of the given input string without allocating extra memory.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A hexadecimal string representing the SHA-256 hash.</returns>
        private static string ComputeSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                // Convert input string to byte array
                var inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the SHA-256 hash
                var hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes into a hexadecimal string
                var result = new StringBuilder(hashBytes.Length * 2);
                foreach (var b in hashBytes)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Custom comparison method for sorting Guids without LINQ.
        /// </summary>
        /// <param name="g1">The first Guid.</param>
        /// <param name="g2">The second Guid.</param>
        /// <returns>An integer indicating the relative order of the two Guids.</returns>
        private static int CompareGuids(Guid g1, Guid g2)
        {
            var g1Bytes = g1.ToByteArray();
            var g2Bytes = g2.ToByteArray();

            for (var i = 0; i < 16; i++)
            {
                if (g1Bytes[i] < g2Bytes[i])
                    return -1;
                if (g1Bytes[i] > g2Bytes[i])
                    return 1;
            }

            return 0;
        }
    }
}