namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ITargetSensor : IHasConfig<ITargetSensorConfig>, ISensor
    {
        public ITargetKey Key { get; }
    }
}