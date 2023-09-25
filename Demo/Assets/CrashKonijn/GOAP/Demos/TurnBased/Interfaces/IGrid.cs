using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Interfaces
{
    public interface IGrid
    {
        ITile[,] GetGrid();
        ITile[] GetWalkableTiles();
        ITile GetTile(Vector3 position);
        ITile[] GetPath(ITile from, ITile to);
        ITile[] GetPath(Vector3 from, Vector3 to);
    }
}