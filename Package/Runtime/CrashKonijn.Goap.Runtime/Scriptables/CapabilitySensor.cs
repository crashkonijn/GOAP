using System;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public abstract class CapabilitySensor
    {
        public ClassRef sensor = new();
    }
}