using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class MonoCapabilityFactoryBase : MonoBehaviour, ICapabilityFactory
    {
        public abstract ICapabilityConfig Create();
    }
}