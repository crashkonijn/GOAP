namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IActionRunState
    {
        bool ShouldStop(IAgent agent);
        bool ShouldPerform(IAgent agent);
        bool IsCompleted(IAgent agent);
        bool MayResolve(IAgent agent);
    }
}