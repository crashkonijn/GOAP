using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased
{
    public class TileTarget : ITarget
    {
        public ITile Tile { get; }
        public Vector3 Position => this.Tile.Position;

        public TileTarget(ITile tile)
        {
            this.Tile = tile;
        }
    }
}