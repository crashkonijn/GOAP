using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class StopAndLog : IActionRunState
    {
        private readonly string message;

        public StopAndLog(string message)
        {
            this.message = message;
        }
        
        public void Update(IAgent agent, IActionContext context)
        {
        }

        public bool ShouldStop(IAgent agent)
        {
            agent.Logger.Log(this.message);
            return true;
        }

        public bool ShouldPerform(IAgent agent)
        {
            return false;
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