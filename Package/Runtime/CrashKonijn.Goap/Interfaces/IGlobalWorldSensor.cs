using CrashKonijn.Goap.Classes;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGlobalWorldSensor : IWorldSensor
    {
        public SenseValue Sense();
    }
}