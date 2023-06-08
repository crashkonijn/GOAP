namespace CrashKonijn.Goap.Interfaces
{
    public interface IAgentDebugger
    {
        string GetInfo(IMonoAgent agent, IComponentReference references);
    }
}