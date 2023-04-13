using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IWorldSensor : IHasConfig<IWorldSensorConfig>
    {
        public IWorldKey Key { get; }
        void Created();
    }
}