using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public interface IConditionBuilder
    {
        IConditionBuilder SetConditionMet(ICondition condition, bool met);
        bool[] Build();
        void Clear();
    }
}