using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IGoapInjector
    {
        void Inject(IAction action);
        void Inject(IGoal goal);
        void Inject(ISensor sensor);
    }
}