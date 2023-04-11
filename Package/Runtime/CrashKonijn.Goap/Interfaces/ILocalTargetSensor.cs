using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.References;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ILocalTargetSensor : ITargetSensor
    {
        public void Update();
        
        public ITarget Sense(IMonoAgent agent, IComponentReference references);
    }
}