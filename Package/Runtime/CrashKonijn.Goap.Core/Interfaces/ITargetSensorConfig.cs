namespace CrashKonijn.Goap.Core
{
    public interface ITargetSensorConfig : IClassConfig
    {
        public ITargetKey Key { get; }
    }
}