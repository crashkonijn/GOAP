using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
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

        public T[] Get<T>()
            where T : IHoldable
        {
            return this.items.Where(x => x is T).Cast<T>().ToArray();
        }

        public bool Any<T>()
            where T : IHoldable
        {
            return this.items.Any(x => x is T);
        }

        public int Count()
        {
            return this.items.Count;
        }
    }
}