using System.Collections.Generic;

namespace CrashKonijn.GOAP.Demos.TurnBased.Interfaces
{
    public interface IPathfinding
    {
        ITile[] FindPath(ITile[,] grid, ITile startTile, ITile targetTile);
    }
}