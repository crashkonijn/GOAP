namespace CrashKonijn.Agent.Core
{
    public interface IActionDisabler
    {
        bool IsDisabled(IAgent agent);
    }
}
