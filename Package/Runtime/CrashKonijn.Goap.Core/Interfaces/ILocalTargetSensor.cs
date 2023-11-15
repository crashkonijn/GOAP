namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ILocalTargetSensor : ITargetSensor
    {
        public void Update();
        
        public ITarget Sense(IMonoAgent agent, IComponentReference references);
    }
}