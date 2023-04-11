using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ItemFactory : MonoBehaviour
    {
        private int count;
        
        public List<ItemBase> items = new();

        public T Instantiate<T>()
            where T : ItemBase
        {
            // Find the item in the list
            var item = this.items.FirstOrDefault(x => x is T);
            
            if (item == null)
                throw new System.Exception($"Item of type {typeof(T).Name} not found in factory");
            
            // Instantiate the item
            var instance = Instantiate(item);

            var name = $"{typeof(T).Name} - {this.count}";
            instance.transform.name = name;
            instance.DebugName = name;

            this.count++;
            
            return instance as T;
        }
    }
}