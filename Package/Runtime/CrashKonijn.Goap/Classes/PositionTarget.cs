using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
{
    public class PositionTarget : ITarget
    {
        public Vector3 Position { get; }

        public PositionTarget(Vector3 position)
        {
            this.Position = position;
        }
    }
}