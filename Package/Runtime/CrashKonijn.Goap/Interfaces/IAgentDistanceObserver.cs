namespace CrashKonijn.Goap.Interfaces
{
    public interface IAgentDistanceObserver
    {
        float GetDistance(IMonoAgent agent, ITarget target, IComponentReference reference);
    }
}