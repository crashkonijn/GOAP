namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IActionContext
    {
        float DeltaTime { get; set; }
        public bool IsInRange { get; set; }
    }
}