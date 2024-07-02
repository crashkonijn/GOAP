using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ContinueOrResolveActionRunState : IActionRunState
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
            return true;
        }
        
        public bool IsRunning()
        {
            return false;
        }
    }
}