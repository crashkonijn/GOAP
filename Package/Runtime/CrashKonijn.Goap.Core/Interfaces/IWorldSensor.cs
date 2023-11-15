namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IWorldSensor : IHasConfig<IWorldSensorConfig>, ISensor
    {
        public IWorldKey Key { get; }
    }
}