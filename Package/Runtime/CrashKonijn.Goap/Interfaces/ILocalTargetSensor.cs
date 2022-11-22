using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ILocalTargetSensor : ITargetSensor
    {
        public ITarget Sense(IMonoAgent agent);
    }
}