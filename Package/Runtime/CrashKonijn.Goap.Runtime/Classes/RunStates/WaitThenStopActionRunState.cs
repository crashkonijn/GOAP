using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WaitThenStopActionRunState : IActionRunState
    {
        private float time;
        private readonly bool mayResolve;

        public WaitThenStopActionRunState(float time, bool mayResolve)
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
            return this.time <= 0f;
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
            return this.mayResolve;
        }
        
        public bool IsRunning()
        {
            return this.time > 0f;
        }
    }
}