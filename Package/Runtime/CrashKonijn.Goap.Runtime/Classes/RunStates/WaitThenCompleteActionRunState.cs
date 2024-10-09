using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WaitThenCompleteActionRunState : ActionRunState
    {
        private float time;
        private readonly bool mayResolve;

        public WaitThenCompleteActionRunState(float time, bool mayResolve)
        {
            this.time = time;
            this.mayResolve = mayResolve;
        }

        public override void Update(IAgent agent, IActionContext context)
        {
            this.time -= context.DeltaTime;
        }

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
            return this.time <= 0f;
        }

        public override bool MayResolve(IAgent agent)
        {
            return this.mayResolve;
        }

        public override bool IsRunning()
        {
            return this.time > 0f;
        }
    }
}
