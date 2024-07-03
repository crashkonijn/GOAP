namespace CrashKonijn.Agent.Core
{
    public interface IActionProvider
    {
        IActionReceiver Receiver { get; set; }
        void ResolveAction();
    }
}