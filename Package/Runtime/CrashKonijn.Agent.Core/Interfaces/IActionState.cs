namespace CrashKonijn.Agent.Core
{
    public interface IActionState
    {
        bool HasPerformed { get; }
        IAction Action { get; }
        public IAction PreviousAction { get; }
        IActionRunState RunState { get; set; }
        IActionData Data { get; }

        void SetAction(IAction action, IActionData data);
        void Reset();
    }
}