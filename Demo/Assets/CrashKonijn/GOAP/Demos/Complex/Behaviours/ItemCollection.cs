using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ItemCollection : MonoBehaviour
    {
        private List<IHoldable> items = new();
        
        public void Add(IHoldable item)
        {
            this.items.Add(item);
        }
        
        public void Remove(IHoldable item)
        {
            this.items.Remove(item);
        }
        
        public IHoldable[] All()
        {
            return this.items.ToArray();
        }

        public IHoldable[] Filtered(bool canBeHeld, bool canBeInBox, GameObject canBeClaimedBy = null)
        {
            if (canBeInBox && canBeHeld && canBeClaimedBy == null)
                return this.All();

            var items = this.items as IEnumerable<IHoldable>;
            
            if (!canBeHeld)
                items = items.Where(x => x.IsHeld == false);
            
            if (!canBeInBox)
                items = items.Where(x => x.IsInBox == false);
            
            if (canBeClaimedBy == null)
                items = items.Where(x => x.IsClaimed == false);
            
            if (canBeClaimedBy != null)
                items = items.Where(x => x.IsClaimed == false || x.IsClaimedBy == canBeClaimedBy);

            return items.ToArray();
        }

        public T[] Get<T>()
            where T : IHoldable
        {
            return this.items.Where(x => x is T).Cast<T>().ToArray();
        }
        
        public T[] GetFiltered<T>(bool canBeHeld, bool canBeInBox, GameObject canBeClaimedBy = null)
            where T : IHoldable
        {
            return this.Filtered(canBeHeld, canBeInBox, canBeClaimedBy).Where(x => x is T).Cast<T>().ToArray();
        }

        public bool Any<T>()
            where T : IHoldable
        {
            return this.items.Any(x => x is T);
        }

        public int Count(bool canBeInBox, bool canBeHeld)
        {
            return this.Filtered(canBeInBox, canBeHeld).Length;
        }

        public IHoldable Closest(Vector3 position, bool canBeInBox, bool canBeHeld, GameObject canBeClaimedBy = null)
        {
            var filteredItems = this.Filtered(canBeInBox, canBeHeld, canBeClaimedBy);
            IHoldable closest = null;
            var closestDistance = float.MaxValue; // Start with the largest possible distance

            foreach (var item in filteredItems)
            {
                var distance = Vector3.Distance(item.gameObject.transform.position, position);
                
                if (!(distance < closestDistance))
                    continue;
                
                closest = item;
                closestDistance = distance;
            }

            return closest;
        }
    }
}