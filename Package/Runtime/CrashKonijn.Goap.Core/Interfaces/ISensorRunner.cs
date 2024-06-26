namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ISensorRunner
    {
        void Update();
        void Update(IGoapAction action);
        void SenseGlobal();
        void SenseGlobal(IGoapAction action);
        void SenseLocal(IMonoGoapAgent agent);
        void SenseLocal(IMonoGoapAgent agent, IGoapAction action);
    }
}