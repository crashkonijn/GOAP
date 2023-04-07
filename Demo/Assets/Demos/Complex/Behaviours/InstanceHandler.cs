using System.Collections.Generic;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class InstanceHandler : MonoBehaviour
    {
        private Queue<IHoldable> queue = new();

        private void LateUpdate()
        {
            while (this.queue.Count > 0)
            {
                var item = this.queue.Dequeue();
                Destroy(item.gameObject);
            }
        }

        public void Destroy(IHoldable item)
        {
            if (item == null)
                return;
            
            this.queue.Enqueue(item);
            item.gameObject.SetActive(false);
        }
    }
}