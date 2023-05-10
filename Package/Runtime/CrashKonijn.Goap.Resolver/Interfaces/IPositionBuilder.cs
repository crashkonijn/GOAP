using Unity.Mathematics;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IPositionBuilder
    {
        PositionBuilder SetPosition(IAction action, Vector3 position);
        float3[] Build();
        void Clear();
    }
}