namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ISensorRunner
    {
        void Update();
        IGlobalWorldData SenseGlobal();
        ILocalWorldData SenseLocal(IGlobalWorldData worldData, IMonoAgent agent);
    }
}