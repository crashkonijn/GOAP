using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.RunStates
{
    public class WaitThenStopActionRunState : IActionRunState
    {
        private readonly float time;
        private readonly bool mayResolve;

        public WaitThenStopActionRunState(float time, bool mayResolve)
        {
            this.time = time;
            this.mayResolve = mayResolve;
        }

        public bool ShouldStop(IAgent agent)
        {
            return agent.Timers.Action.IsRunningFor(this.time);
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
    }
}