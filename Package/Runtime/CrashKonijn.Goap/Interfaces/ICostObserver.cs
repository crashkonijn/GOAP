namespace CrashKonijn.Goap.Interfaces
{
    public interface ICostObserver : Resolver.Interfaces.ICostObserver
    {
        void SetWorldData(IWorldData worldData);
    }
}