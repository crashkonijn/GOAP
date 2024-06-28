using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public class ActionContext : IActionContext
    {
        public float DeltaTime { get; set; }
        public bool IsInRange { get; set; }
    }
}