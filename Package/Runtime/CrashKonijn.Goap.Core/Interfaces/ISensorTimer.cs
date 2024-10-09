using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface ISensorTimer
    {
        bool ShouldSense(ITimer timer);
    }
}