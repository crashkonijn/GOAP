namespace CrashKonijn.Goap.Core
{
    public interface ICostObserver
    {
        float GetCost(IConnectable current, IConnectable[] path);
        void SetWorldData(IWorldData worldData);
    }
}
