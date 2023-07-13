using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace Demos
{
    public static class Extensions
    {
        public static GameObject Closest(this IEnumerable<GameObject> items, Vector3 position)
        {
            return items
                .OrderBy(x => Vector3.Distance(x.transform.position, position))
                .FirstOrDefault();
        }
        
        public static T Closest<T>(this IEnumerable<T> items, Vector3 position)
            where T : MonoBehaviour
        {
            return items
                .OrderBy(x => Vector3.Distance(x.transform.position, position))
                .FirstOrDefault();
        }
        
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            List<T> list = new List<T>(enumerable);
        
            if (list.Count == 0)
            {
                throw new InvalidOperationException("The collection is empty.");
            }

            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}