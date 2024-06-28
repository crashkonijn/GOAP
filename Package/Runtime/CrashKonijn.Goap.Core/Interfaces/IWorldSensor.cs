namespace CrashKonijn.Goap.Core
{
    public interface IWorldSensor : IHasConfig<IWorldSensorConfig>, ISensor
    {
        public IWorldKey Key { get; }
    }
}