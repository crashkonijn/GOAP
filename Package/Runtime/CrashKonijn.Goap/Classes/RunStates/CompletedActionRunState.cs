using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.RunStates
{
    public class CompletedActionRunState : IActionRunState
    {
        public bool ShouldStop(IAgent agent)
        {
            return false;
        }

        public bool ShouldPerform(IAgent agent)
        {
            return false;
        }

        public bool IsCompleted(IAgent agent)
        {
            return true;
        }

        public bool MayResolve(IAgent agent)
        {
            return false;
        }
    }
}