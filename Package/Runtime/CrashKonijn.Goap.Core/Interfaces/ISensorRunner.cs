namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ISensorRunner
    {
        void Update();
        void SenseGlobal();
        void SenseLocal(IMonoAgent agent);
    }
}