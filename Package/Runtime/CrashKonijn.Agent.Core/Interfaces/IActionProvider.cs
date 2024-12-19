namespace CrashKonijn.Agent.Core
{
    public interface IActionProvider
    {
        IActionReceiver Receiver { get; set; }
        void ResolveAction();
        bool IsDisabled(IAction action);
        void Enable(IAction action);
        void Disable(IAction action, IActionDisabler disabler);
    }
}