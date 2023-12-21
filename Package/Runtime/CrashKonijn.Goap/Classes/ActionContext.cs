using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public struct ActionContext : IActionContext
    {
        public float DeltaTime { get; set; }
    }
}