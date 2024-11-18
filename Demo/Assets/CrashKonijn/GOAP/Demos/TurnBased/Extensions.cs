using System;
using System.Collections.Generic;

namespace CrashKonijn.GOAP.Demos.TurnBased
{
    public static class Extensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = new List<T>(enumerable);

            if (list.Count == 0)
            {
                throw new InvalidOperationException("The collection is empty.");
            }

            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}
