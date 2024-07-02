using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
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