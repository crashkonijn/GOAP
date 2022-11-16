using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ILocalWorldSensor : IWorldSensor
    {
        public bool Sense(Agent agent);
    }
}