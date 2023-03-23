using System;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public abstract class ItemBase : MonoBehaviour, IHoldable
    {
        private ItemCollection collection;
        public bool IsHeld { get; private set; }

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

        public void Pickup()
        {
            this.IsHeld = true;
            this.collection.Remove(this);
        }

        public void Drop()
        {
            this.IsHeld = false;
            this.collection.Add(this);
        }
    }
}