namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ITargetSensor : IHasConfig<ITargetSensorConfig>
    {
        public ITargetKey Key { get; }
        void Created();
    }
}