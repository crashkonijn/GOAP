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
            
            if (!this.items.Contains(item))
                this.items.Add(item);
        }
        
        public void Hold<T>(T item)
            where T : IHoldable
        {
            item.Pickup(true);

            item.gameObject.transform.position = this.transform.position + new Vector3(0f, 0.1f, -0.2f);
            item.gameObject.transform.parent = this.transform;

            if (!this.items.Contains(item))
                this.items.Add(item);
        }

        public T[] Get<T>()
        {
            return this.items.Where(x => x is T).Cast<T>().ToArray();
        }
        
        public void Remove<T>(T item)
            where T : IHoldable
        {
            this.items.Remove(item);
            
            if (item == null || item.gameObject == null)
                return;
            
            item.gameObject.transform.parent = null;
        }
        
        public bool Has<T>()
            where T : IHoldable
        {
            return this.items.Any(x => x is T);
        }
        
        public int Count<T>()
            where T : IHoldable
        {
            return this.items.Count(x => x is T);
        }
        
        public bool Has(Type type, int amount)
        {
            return this.items.Count(x => x.GetType().IsInstanceOfType(type)) >= amount;
        }
    }
}