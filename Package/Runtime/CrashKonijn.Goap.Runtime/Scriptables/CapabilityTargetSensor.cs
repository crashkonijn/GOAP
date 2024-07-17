using System;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class CapabilityTargetSensor : CapabilitySensor
    {
        public ClassRef targetKey = new();

        public override string ToString() => $"{this.sensor.Name} ({this.targetKey.Name})";
    }
}