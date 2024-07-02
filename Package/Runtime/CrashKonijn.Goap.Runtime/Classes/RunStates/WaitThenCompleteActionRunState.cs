using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WaitThenCompleteActionRunState : IActionRunState
    {
        private float time;
        private readonly bool mayResolve;

        public WaitThenCompleteActionRunState(float time, bool mayResolve)
        {
            this.time = time;
            this.mayResolve = mayResolve;
        }

        public void Update(IAgent agent, IActionContext context)
        {
            this.time -= context.DeltaTime;
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
            return this.time <= 0f;
        }

        public bool MayResolve(IAgent agent)
        {
            return this.mayResolve;
        }
        
        public bool IsRunning()
        {
            return this.time > 0f;
        }
    }
}