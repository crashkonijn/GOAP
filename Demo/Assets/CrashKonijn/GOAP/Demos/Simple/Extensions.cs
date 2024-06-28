using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demos
{
    public static class Extensions
    {
        public static GameObject Closest(this IEnumerable<GameObject> items, Vector3 position)
        {
            GameObject closest = null;
            var closestDistance = float.MaxValue;

            foreach (var item in items)
            {
                var distance = Vector3.Distance(item.transform.position, position);
                if (!(distance < closestDistance))
                    continue;
                
                closest = item;
                closestDistance = distance;
            }

            return closest;
        }
        
        public static T Closest<T>(this IEnumerable<T> items, Vector3 position)
            where T : MonoBehaviour
        {
            T closest = null;
            var closestDistance = float.MaxValue;

            foreach (var item in items)
            {
                if (item == null)
                    continue;
                
                var distance = Vector3.Distance(item.transform.position, position);
                if (!(distance < closestDistance))
                    continue;
                
                closest = item;
                closestDistance = distance;
            }

            return closest;
        }
    }
}