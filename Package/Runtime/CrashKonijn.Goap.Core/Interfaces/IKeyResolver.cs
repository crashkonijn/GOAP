namespace CrashKonijn.Goap.Core
{
    public interface IKeyResolver
    {
        string GetKey(IConnectable action, ICondition condition);
        string GetKey(IConnectable action, IEffect effect);
        void SetWorldData(IWorldData globalWorldData);
    }
}
