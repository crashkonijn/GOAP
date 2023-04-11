using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ITargetSensor : IHasConfig<ITargetSensorConfig>
    {
        public ITargetKey Key { get; }
        void Created();
    }
}