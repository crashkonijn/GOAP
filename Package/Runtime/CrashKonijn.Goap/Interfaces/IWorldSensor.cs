using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IWorldSensor
    {
        public WorldKey Key { get; }
        public void SetConfig(WorldSensorConfig config);
    }
}