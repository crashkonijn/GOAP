using CrashKonijn.GOAP.Demos.TurnBased.Behaviours;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Interfaces
{
    public interface ITile
    {
        int X { get; }
        int Y { get; }
        bool IsWalkable { get; }
        Vector3 Position { get; }
        
        void Initialize(int x, int y, IGrid grid);
        void SetWalkable(bool isWalkable);
    }
}