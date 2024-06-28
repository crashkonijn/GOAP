using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IAgentTypeJobRunner
    {
        void Run(HashSet<IMonoGoapActionProvider> queue);
        void Complete();
        void Dispose();
        IGraph GetGraph();
    }
}