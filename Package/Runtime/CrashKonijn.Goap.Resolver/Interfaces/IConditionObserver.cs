namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IConditionObserver
    {
        bool IsMet(ICondition condition);
        bool IsMet(IEffect effect);
    }
}