using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public abstract class ItemBase : MonoBehaviour, IHoldable
    {
        private ItemCollection collection;
        
        [field: SerializeField]
        public bool IsHeld { get; private set; }
        
        [field: SerializeField]
        public bool IsInBox { get; private set; }
        
        [field: SerializeField]
        public bool IsClaimed { get; private set; }

        public string DebugName { get; set; }

        public void Awake()
        {
            this.collection = FindObjectOfType<ItemCollection>();
        }

        public void OnEnable()
        {
            this.collection.Add(this);
        }

        public void OnDisable()
        {
            this.collection.Remove(this);
        }

        public void Claim()
        {
            this.IsClaimed = true;
        }

        public void Pickup(bool visible = false)
        {
            this.IsHeld = true;
            this.IsInBox = false;
            this.IsClaimed = true;

            if (this == null || this.gameObject == null)
                return;
            
            foreach (var renderer in this.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.enabled = visible;
            }
        }

        public void Drop(bool inBox = false)
        {
            this.IsHeld = false;
            this.IsInBox = inBox;
            this.IsClaimed = false;
            
            foreach (var renderer in this.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.enabled = true;
            }
        }
    }
}