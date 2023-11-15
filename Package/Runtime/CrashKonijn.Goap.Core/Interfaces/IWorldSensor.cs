namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IWorldSensor : IHasConfig<IWorldSensorConfig>
    {
        public IWorldKey Key { get; }
        void Created();
    }
}