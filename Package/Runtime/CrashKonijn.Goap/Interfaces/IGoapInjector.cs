using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapInjector
    {
        void Inject(IActionBase action);
        void Inject(IGoalBase action);
    }
}