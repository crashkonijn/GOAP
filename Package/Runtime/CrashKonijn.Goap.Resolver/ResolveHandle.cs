using System.Collections.Generic;
using CrashKonijn.Goap.Resolver.Interfaces;
using Unity.Collections;
using Unity.Jobs;

namespace CrashKonijn.Goap.Resolver
{
    public class ResolveHandle : IResolveHandle
    {
        private readonly GraphResolver graphResolver;
        private JobHandle handle;
        private GraphResolverJob job;
        private RunData runData;

        public ResolveHandle(GraphResolver graphResolver, NativeMultiHashMap<int, int> connections, RunData runData)
        {
            this.graphResolver = graphResolver;
            this.job = new GraphResolverJob
            {
                Connections = connections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob)
            };
        
            this.handle = this.job.Schedule();
        }

        public IAction[] Complete()
        {
            this.handle.Complete();
        
            var results = new List<IAction>();
        
            foreach (var data in this.job.Result)
            {
                results.Add(this.graphResolver.GetAction(data.Index));
            }
        
            this.job.Result.Dispose();
        
            this.job.RunData.IsExecutable.Dispose();
            this.job.RunData.Positions.Dispose();
            this.job.RunData.Costs.Dispose();

            return results.ToArray();
        }
    }
}