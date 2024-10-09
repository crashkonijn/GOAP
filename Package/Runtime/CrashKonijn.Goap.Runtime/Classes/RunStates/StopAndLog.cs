using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class StopAndLog : ActionRunState
    {
        private readonly string message;

        public StopAndLog(string message)
        {
            this.message = message;
        }

        public override void Update(IAgent agent, IActionContext context) { }

        public override bool ShouldStop(IAgent agent)
        {
            agent.Logger.Log(this.message);
            return true;
        }

        public override bool ShouldPerform(IAgent agent)
        {
            return false;
        }

        public override bool IsCompleted(IAgent agent)
        {
            return false;
        }

        public override bool MayResolve(IAgent agent)
        {
            return false;
        }

        public override bool IsRunning()
        {
            return false;
        }
    }
}
