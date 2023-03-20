using System;
using System.Collections.Generic;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ComplexInventoryBehaviour : MonoBehaviour
    {
        private Dictionary<Type, List<object>> inventory = new();
        
        public void Add<T>(T item)
            where T : IHoldable
        {
            item.Pickup();
            
            var type = typeof(T);
            if (!this.inventory.ContainsKey(type))
            {
                this.inventory.Add(type, new List<object>());
            }
            
            this.inventory[type].Add(item);
        }
        
        public void Remove<T>(T item)
            where T : IHoldable
        {
            item.Drop();
            
            var type = typeof(T);
            if (!this.inventory.ContainsKey(type))
            {
                return;
            }
            
            this.inventory[type].Remove(item);
        }
        
        public bool Has<T>()
            where T : IHoldable
        {
            var type = typeof(T);
            if (!this.inventory.ContainsKey(type))
            {
                return false;
            }
            
            return this.inventory[type].Count > 0;
        }
        
        public bool Has(Type type, int amount)
        {
            if (!this.inventory.ContainsKey(type))
            {
                return false;
            }
            
            return this.inventory[type].Count >= amount;
        }
    }
}