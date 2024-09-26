using System;
using System.Collections.Generic;
using System.Text;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public static class GuidCacheKey
    {
        private static readonly StringBuilder StringBuilder = new();

        public static string GenerateKey(List<IGoal> guids)
        {
            if (guids == null || guids.Count == 0)
                throw new ArgumentException("Guid list must not be null or empty.");

            guids.Sort(Sort);

            StringBuilder.Clear();

            foreach (var guid in guids)
            {
                StringBuilder.Append(guid.Index);
                StringBuilder.Append("-");
            }

            return StringBuilder.ToString();
        }

        private static int Sort(IGoal g1, IGoal g2)
        {
            var g1Bytes = g1.Index;
            var g2Bytes = g2.Index;

            if (g1Bytes < g2Bytes)
                return -1;

            if (g1Bytes > g2Bytes)
                return 1;

            return 0;
        }
    }
}
