using CrashKonijn.Goap.Core.Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IPositionBuilder
    {
        IPositionBuilder SetPosition(IConnectable action, Vector3 position);
        float3[] Build();
        void Clear();
    }
}