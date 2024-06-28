using CrashKonijn.Goap.Core;
using Unity.Mathematics;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver
{
    public interface IPositionBuilder
    {
        IPositionBuilder SetPosition(IConnectable action, Vector3? position);
        float3[] Build();
        void Clear();
    }
}