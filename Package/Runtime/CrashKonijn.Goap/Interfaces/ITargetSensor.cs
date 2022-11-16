using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ITargetSensor
    {
        public TargetKey Key { get; }
        
        public void SetConfig(TargetSensorConfig config);
    }
}