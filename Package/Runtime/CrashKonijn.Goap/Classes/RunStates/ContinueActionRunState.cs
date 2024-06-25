using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.RunStates
{
    public class ContinueActionRunState : IActionRunState
    {
        public void Update(IAgent agent, IActionContext context)
        {
            
        }

        public bool ShouldStop(IAgent agent)
        {
            return false;
        }

        public bool ShouldPerform(IAgent agent)
        {
            return true;
        }

        public bool IsCompleted(IAgent agent)
        {
            return false;
        }

        public bool MayResolve(IAgent agent)
        {
            return false;
        }

        public bool IsRunning()
        {
            return false;
        }
    }
}