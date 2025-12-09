using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class CapabilityFactoryBase : ICapabilityFactory
    {
        public abstract ICapabilityConfig Create();
    }
}