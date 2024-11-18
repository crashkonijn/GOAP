namespace CrashKonijn.Goap.Core
{
    public interface IAgentTypeJobRunner
    {
        void Run(IMonoGoapActionProvider[] queue);
        void Complete();
        void Dispose();
        IGraph GetGraph();
    }
}
