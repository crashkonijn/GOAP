using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public class ForeverActionDisabler : IActionDisabler
    {
        public bool IsDisabled(IAgent agent) => true;
    }
}