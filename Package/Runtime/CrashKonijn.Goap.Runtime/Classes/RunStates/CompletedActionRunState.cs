using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class CompletedActionRunState : ActionRunState
    {
        public override void Update(IAgent agent, IActionContext context) { }

        public override bool ShouldStop(IAgent agent)
        {
            return false;
        }

        public override bool ShouldPerform(IAgent agent)
        {
            return false;
        }

        public override bool IsCompleted(IAgent agent)
        {
            return true;
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
