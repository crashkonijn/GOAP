using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class ScriptableCapabilityFactoryBase : ScriptableObject, ICapabilityFactory
    {
        public abstract ICapabilityConfig Create();
    }
}