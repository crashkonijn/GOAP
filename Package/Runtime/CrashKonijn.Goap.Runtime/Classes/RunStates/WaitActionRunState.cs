using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WaitActionRunState : ActionRunState
    {
        private readonly bool mayResolve;
        private float time;

        public WaitActionRunState(float time, bool mayResolve)
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
            return this.time <= 0f;
        }

        public override bool IsCompleted(IAgent agent)
        {
            return false;
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
