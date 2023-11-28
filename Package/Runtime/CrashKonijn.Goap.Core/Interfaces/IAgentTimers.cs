namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentTimers
    {
        ITimer Action { get; }
        ITimer Goal { get; }
        ITimer Resolve { get; }
    }
}