using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class CompletedActionRunState : IActionRunState
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
        
        public bool IsRunning()
        {
            return false;
        }
    }
}