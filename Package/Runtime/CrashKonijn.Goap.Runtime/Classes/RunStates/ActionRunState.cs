using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Runtime
{
    public static class ActionRunState {
        public static readonly IActionRunState Continue = new ContinueActionRunState();
        public static readonly IActionRunState ContinueOrResolve = new ContinueOrResolveActionRunState();
        public static readonly IActionRunState Stop = new StopActionRunState();
        public static readonly IActionRunState Completed = new CompletedActionRunState();
        public static IActionRunState Wait(float time, bool mayResolve = false) => new WaitActionRunState(time, mayResolve);
        public static IActionRunState WaitThenComplete(float time, bool mayResolve = false) => new WaitThenCompleteActionRunState(time, mayResolve);
        public static IActionRunState WaitThenStop(float time, bool mayResolve = false) => new WaitThenStopActionRunState(time, mayResolve);
        public static IActionRunState StopAndLog(string message) => new StopAndLog(message);
    }
}