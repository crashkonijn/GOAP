using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WaitActionRunState : IActionRunState
    {
        private readonly bool mayResolve;
        private float time;

        public WaitActionRunState(float time, bool mayResolve)
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
            return this.time <= 0f;;
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