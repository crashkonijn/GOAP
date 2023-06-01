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

#if UNITY_COLLECTIONS_2_1
        public ResolveHandle(GraphResolver graphResolver, NativeParallelMultiHashMap<int, int> nodeConditions, NativeParallelMultiHashMap<int, int> conditionConnections, RunData runData)
        {
            this.graphResolver = graphResolver;
            this.job = new GraphResolverJob
            {
                NodeConditions = nodeConditions,
                ConditionConnections = conditionConnections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob)
            };
        
            this.handle = this.job.Schedule();
        }
#else
        public ResolveHandle(GraphResolver graphResolver, NativeMultiHashMap<int, int> nodeConditions, NativeMultiHashMap<int, int> conditionConnections, RunData runData)
        {
            this.graphResolver = graphResolver;
            this.job = new GraphResolverJob
            {
                NodeConditions = nodeConditions,
                ConditionConnections = conditionConnections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob)
            };
        
            this.handle = this.job.Schedule();
        }
#endif

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
            this.job.RunData.ConditionsMet.Dispose();

            return results.ToArray();
        }
    }
}