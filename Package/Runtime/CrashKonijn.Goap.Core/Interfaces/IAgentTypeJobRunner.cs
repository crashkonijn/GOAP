using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentTypeJobRunner
    {
        void Run(HashSet<IMonoAgent> queue);
        void Complete();
        void Dispose();
        IGraph GetGraph();
    }
}