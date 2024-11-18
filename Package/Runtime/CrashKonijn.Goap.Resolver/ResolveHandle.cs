using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using Unity.Collections;
using Unity.Jobs;

namespace CrashKonijn.Goap.Resolver
{
    public class ResolveHandle : IResolveHandle
    {
        private readonly GraphResolver graphResolver;
        private JobHandle handle;
        private GraphResolverJob job;
        private readonly List<IConnectable> results = new();

        public ResolveHandle(GraphResolver graphResolver)
        {
            this.graphResolver = graphResolver;
        }

#if UNITY_COLLECTIONS_2_1
        public ResolveHandle Start(NativeParallelMultiHashMap<int, int> nodeConditions, NativeParallelMultiHashMap<int, int> conditionConnections, RunData runData)
        {
            this.job = new GraphResolverJob
            {
                NodeConditions = nodeConditions,
                ConditionConnections = conditionConnections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob),
                PickedGoal = new NativeList<NodeData>(Allocator.TempJob)
            };

            this.handle = this.job.Schedule();

            return this;
        }
#else
        public ResolveHandle Start(NativeMultiHashMap<int, int> nodeConditions, NativeMultiHashMap<int, int> conditionConnections, RunData runData)
        {
            this.job = new GraphResolverJob
            {
                NodeConditions = nodeConditions,
                ConditionConnections = conditionConnections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob),
                PickedGoal = new NativeList<NodeData>(Allocator.TempJob),
            };

            this.handle = this.job.Schedule();

            return this;
        }
#endif

        public JobResult Complete()
        {
            this.handle.Complete();

            this.results.Clear();

            foreach (var data in this.job.Result)
            {
                this.results.Add(this.graphResolver.GetAction(data.Index));
            }

            IGoal goal = default;

            if (this.job.PickedGoal.Length > 0)
                goal = this.graphResolver.GetGoal(this.job.PickedGoal[0].Index);

            this.job.Result.Dispose();
            this.job.PickedGoal.Dispose();

            this.job.RunData.StartIndex.Dispose();
            this.job.RunData.IsEnabled.Dispose();
            this.job.RunData.IsExecutable.Dispose();
            this.job.RunData.Positions.Dispose();
            this.job.RunData.Costs.Dispose();
            this.job.RunData.ConditionsMet.Dispose();

            this.graphResolver.Release(this);

            return new JobResult
            {
                Actions = this.results.ToArray(),
                Goal = goal,
            };
        }
    }

    public class JobResult
    {
        public IConnectable[] Actions { get; set; }
        public IGoal Goal { get; set; }
    }
}
