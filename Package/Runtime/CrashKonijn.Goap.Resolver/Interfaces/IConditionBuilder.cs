namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IConditionBuilder
    {
        IConditionBuilder SetConditionMet(ICondition condition, bool met);
        bool[] Build();
        void Clear();
    }
}