using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapInjector
    {
        void Inject(IActionBase action);
        void Inject(IGoalBase goal);
        void Inject(IWorldSensor worldSensor);
        void Inject(ITargetSensor targetSensor);
    }
}