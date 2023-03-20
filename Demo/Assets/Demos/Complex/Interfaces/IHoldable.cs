using UnityEngine;

namespace Demos.Complex.Interfaces
{
    public interface IHoldable
    {
        bool IsHeld { get; }

        void Pickup();
        void Drop();
    }
}