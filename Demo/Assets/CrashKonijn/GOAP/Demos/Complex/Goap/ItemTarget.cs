using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Goap
{
    public class ItemTarget : ITarget
    {
        public IHoldable Item { get; }
        public Vector3 Position => this.Item?.gameObject.transform.position ?? Vector3.zero;

        public ItemTarget(IHoldable item)
        {
            this.Item = item;
        }
    }
}