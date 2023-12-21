namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ICostObserver 
    {
        float GetCost(IConnectable current, IConnectable[] path);
        void SetWorldData(IWorldData worldData);
    }
}