namespace CrashKonijn.Goap.Core
{
    public interface ITargetSensor : IHasConfig<ITargetSensorConfig>, ISensor
    {
        public ITargetKey Key { get; }
    }
}