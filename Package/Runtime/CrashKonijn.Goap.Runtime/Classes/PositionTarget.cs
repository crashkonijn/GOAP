using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class PositionTarget : ITarget
    {
        public Vector3 Position { get; private set; }

        public PositionTarget(Vector3 position)
        {
            this.Position = position;
        }

        public PositionTarget SetPosition(Vector3 position)
        {
            this.Position = position;
            return this;
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
