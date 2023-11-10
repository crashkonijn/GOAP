
namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ILocalWorldSensor : IWorldSensor
    {
        public void Update();
        public SenseValue Sense(IMonoAgent agent, IComponentReference references);
    }
}