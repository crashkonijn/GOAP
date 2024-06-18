namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ISensorRunner
    {
        void Update();
        void Update(IAction action);
        void SenseGlobal();
        void SenseGlobal(IAction action);
        void SenseLocal(IMonoAgent agent);
        void SenseLocal(IMonoAgent agent, IAction action);
    }
}