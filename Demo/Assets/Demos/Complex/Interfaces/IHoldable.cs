using UnityEngine;

namespace Demos.Complex.Interfaces
{
    public interface IHoldable
    {
        public GameObject gameObject { get; }
        bool IsHeld { get; }

        void Pickup();
        void Drop();
    }
}