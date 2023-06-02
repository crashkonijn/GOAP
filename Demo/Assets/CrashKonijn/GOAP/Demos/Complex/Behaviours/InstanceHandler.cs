using System.Collections.Generic;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class InstanceHandler : MonoBehaviour
    {
        private List<GameObject> queue = new();

        private void LateUpdate()
        {
            foreach (var item in this.queue)
            {
                Destroy(item.gameObject);
            }
            
            this.queue.Clear();
        }

        public void QueueForDestroy(IHoldable item)
        {
            if (item == null)
                return;

            if (item.gameObject == null)
                return;
            
            this.QueueForDestroy(item.gameObject);
        }

        public void QueueForDestroy(GameObject item)
        {
            if (this.queue.Contains(item))
            {
                Debug.LogError("Item already queued for destruction");
                return;
            }
            
            if (item == null)
                return;
            
            this.queue.Add(item);
            item.gameObject.SetActive(false);
        }
    }
}