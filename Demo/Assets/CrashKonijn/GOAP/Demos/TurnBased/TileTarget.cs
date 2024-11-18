using CrashKonijn.Agent.Core;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased
{
    public class TileTarget : ITarget
    {
        public ITile Tile { get; }
        public Vector3 Position => this.Tile.Position;

        public bool IsValid()
        {
            if (this.Tile.IsNull())
                return false;

            return true;
        }

        public TileTarget(ITile tile)
        {
            this.Tile = tile;
        }
    }
}
