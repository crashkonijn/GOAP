namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoapInjector
    {
        void Inject(IAction action);
        void Inject(IGoal goal);
        void Inject(ISensor sensor);
    }
}