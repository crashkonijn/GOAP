using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using Unity.Collections;

namespace CrashKonijn.Goap.Resolver
{
    public class GraphResolver : IGraphResolver
    {
        private readonly List<INode> indexList;
        private readonly List<IConnectable> actionIndexList;

        private readonly List<INodeCondition> conditionList;
        private readonly List<ICondition> conditionIndexList;

#if UNITY_COLLECTIONS_2_1
        private NativeParallelMultiHashMap<int, int> nodeConditions;
        private NativeParallelMultiHashMap<int, int> conditionConnections;
#else
        // Dictionary<ActionIndex, ConditionIndex[]>
        private NativeMultiHashMap<int, int> nodeConditions;

        // Dictionary<ConditionIndex, NodeIndex[]>
        private NativeMultiHashMap<int, int> conditionConnections;
#endif

        private Graph graph;
        private Queue<ResolveHandle> handles = new();

        public GraphResolver(IConnectable[] actions, IKeyResolver keyResolver)
        {
            this.graph = new GraphBuilder(keyResolver).Build(actions);

            this.indexList = this.graph.AllNodes.ToList();
            this.actionIndexList = this.indexList.Select(x => x.Action).ToList();

            this.conditionList = this.indexList.SelectMany(x => x.Conditions).ToList();
            this.conditionIndexList = this.conditionList.Select(x => x.Condition).ToList();

            this.CreateNodeConditions();
            this.CreateConditionConnections();
        }

        private void CreateNodeConditions()
        {
#if UNITY_COLLECTIONS_2_1
            var map = new NativeParallelMultiHashMap<int, int>(this.indexList.Count, Allocator.Persistent);
#else
            var map = new NativeMultiHashMap<int, int>(this.indexList.Count, Allocator.Persistent);
#endif

            for (var i = 0; i < this.indexList.Count; i++)
            {
                var conditions = this.indexList[i].Conditions
                    .Select(x => this.conditionIndexList.IndexOf(x.Condition));

                foreach (var condition in conditions)
                {
                    map.Add(i, condition);
                }
            }

            this.nodeConditions = map;
        }

        private void CreateConditionConnections()
        {
#if UNITY_COLLECTIONS_2_1
            var map = new NativeParallelMultiHashMap<int, int>(this.conditionIndexList.Count, Allocator.Persistent);
#else
            var map = new NativeMultiHashMap<int, int>(this.conditionIndexList.Count, Allocator.Persistent);
#endif

            for (var i = 0; i < this.conditionIndexList.Count; i++)
            {
                var connections = this.conditionList[i].Connections
                    .Select(x => this.indexList.IndexOf(x));

                foreach (var connection in connections)
                {
                    map.Add(i, connection);
                }
            }

            this.conditionConnections = map;
        }

        public IResolveHandle StartResolve(RunData runData)
        {
            return this.GetResolveHandle().Start(this.nodeConditions, this.conditionConnections, runData);
        }

        public IEnabledBuilder GetEnabledBuilder()
        {
            return new EnabledBuilder(this.actionIndexList);
        }

        public IExecutableBuilder GetExecutableBuilder()
        {
            return new ExecutableBuilder(this.actionIndexList);
        }

        public IPositionBuilder GetPositionBuilder()
        {
            return new PositionBuilder(this.actionIndexList);
        }

        public ICostBuilder GetCostBuilder()
        {
            return new CostBuilder(this.actionIndexList);
        }

        public IConditionBuilder GetConditionBuilder()
        {
            return new ConditionBuilder(this.conditionIndexList);
        }

        public IGraph GetGraph()
        {
            return this.graph;
        }

        public int GetIndex(IConnectable action) => this.actionIndexList.IndexOf(action);
        public IGoapAction GetAction(int index) => this.actionIndexList[index] as IGoapAction;
        public IGoal GetGoal(int index) => this.actionIndexList[index] as IGoal;

        public void Dispose()
        {
            this.nodeConditions.Dispose();
            this.conditionConnections.Dispose();
        }

        private ResolveHandle GetResolveHandle()
        {
            if (this.handles.TryDequeue(out var handle))
                return handle;

            return new ResolveHandle(this);
        }

        public void Release(ResolveHandle handle)
        {
            this.handles.Enqueue(handle);
        }
    }
}
