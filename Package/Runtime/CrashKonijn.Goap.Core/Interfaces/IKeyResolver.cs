namespace CrashKonijn.Goap.Core
{
    public interface IKeyResolver
    {
        string GetKey(ICondition condition);
        string GetKey(IEffect effect);
        bool AreConflicting(IEffect effect, ICondition condition);
        void SetWorldData(IWorldData globalWorldData);
    }
}
