namespace CrashKonijn.Goap.Interfaces
{
    public interface ICostObserver : LamosInteractive.Goap.Interfaces.ICostObserver
    {
        void SetWorldData(IWorldData worldData);
    }
}