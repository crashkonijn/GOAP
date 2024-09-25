using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Goap
{
    public class ItemTarget : ITarget
    {
        public IHoldable Item { get; private set; }
        public Vector3 Position => this.Item?.gameObject.transform.position ?? Vector3.zero;

        public ItemTarget(IHoldable item)
        {
            this.Item = item;
        }
        
        public ItemTarget SetItem(IHoldable item)
        {
            this.Item = item;
            return this;
        }
        
        public bool IsValid()
        {
            return this.Item != null;
        }
    }
}