using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ILocalWorldSensor : IWorldSensor
    {
        public void Update();
        public bool Sense(IMonoAgent agent);
    }
}