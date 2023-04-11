using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ILocalWorldSensor : IWorldSensor
    {
        public void Update();
        public SenseValue Sense(IMonoAgent agent, IComponentReference references);
    }
}