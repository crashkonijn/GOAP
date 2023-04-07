using UnityEngine;

namespace Demos.Complex.Interfaces
{
    public interface IHoldable
    {
        public GameObject gameObject { get; }
        bool IsHeld { get; }
        bool IsInBox { get; }
        bool IsClaimed { get; }

        void Claim();
        void Pickup(bool visible = false);
        void Drop(bool inBox = false);
    }
}