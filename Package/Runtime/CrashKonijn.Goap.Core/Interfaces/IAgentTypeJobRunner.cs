using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentTypeJobRunner
    {
        void Run(HashSet<IMonoGoapAgent> queue);
        void Complete();
        void Dispose();
        IGraph GetGraph();
    }
}