using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class ActionContext : IActionContext
    {
        public float DeltaTime { get; set; }
        public bool IsInRange { get; set; }
    }
}