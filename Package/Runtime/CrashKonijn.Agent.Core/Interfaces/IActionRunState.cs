namespace CrashKonijn.Agent.Core
{
    public interface IActionRunState
    {
        void Update(IAgent agent, IActionContext context);
        bool ShouldStop(IAgent agent);
        bool ShouldPerform(IAgent agent);
        bool IsCompleted(IAgent agent);
        bool MayResolve(IAgent agent);
        bool IsRunning();
    }
}