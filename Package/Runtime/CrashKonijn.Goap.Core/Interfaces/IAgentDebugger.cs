namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentDebugger
    {
        string GetInfo(IMonoAgent agent, IComponentReference references);
    }
}