using System;
using System.Collections.Generic;
using System.Linq;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ComplexInventoryBehaviour : MonoBehaviour
    {
        private List<IHoldable> items = new();
        
        public void Add<T>(T item)
            where T : IHoldable
        {
            item.Pickup();
            
            this.items.Add(item);
        }

        public T[] Get<T>()
        {
            return this.items.Where(x => x is T).Cast<T>().ToArray();
        }
        
        public void Remove<T>(T item)
            where T : IHoldable
        {
            item.Drop();
            
            this.items.Remove(item);
        }
        
        public bool Has<T>()
            where T : IHoldable
        {
            return this.items.Any(x => x is T);
        }
        
        public bool Has(Type type, int amount)
        {
            return this.items.Count(x => x.GetType().IsInstanceOfType(type)) >= amount;
        }
    }
}