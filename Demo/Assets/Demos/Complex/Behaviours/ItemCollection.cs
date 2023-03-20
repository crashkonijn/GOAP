using System.Collections.Generic;
using System.Linq;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ItemCollection : MonoBehaviour
    {
        private List<ItemBase> items = new();
        
        public void Add(ItemBase item)
        {
            this.items.Add(item);
        }
        
        public void Remove(ItemBase item)
        {
            this.items.Remove(item);
        }

        public T[] Get<T>()
            where T : ItemBase
        {
            return this.items.Where(x => x is T).Cast<T>().ToArray();
        }

        public bool Any<T>()
            where T : ItemBase
        {
            return this.items.Any(x => x is T);
        }
    }
}