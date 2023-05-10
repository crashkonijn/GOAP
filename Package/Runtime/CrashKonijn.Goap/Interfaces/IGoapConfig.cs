using CrashKonijn.Goap.Observers;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapConfig
    {
        IConditionObserver ConditionObserver { get; set; }
        IKeyResolver KeyResolver { get; set; }
        IGoapInjector GoapInjector { get; set; }
    }
}