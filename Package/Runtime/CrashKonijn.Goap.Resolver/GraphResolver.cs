using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Resolver.Interfaces;
using CrashKonijn.Goap.Resolver.Models;
using Unity.Collections;
using Unity.Jobs;
using IAction = CrashKonijn.Goap.Resolver.Interfaces.IAction;

namespace CrashKonijn.Goap.Resolver
{
    public class GraphResolver : IGraphResolver
    {
        private readonly List<Node> indexList;
        private readonly List<IAction> actionIndexList;
        
#if UNITY_COLLECTIONS_2_1
        private NativeParallelMultiHashMap<int, int> map;
#else
        private NativeMultiHashMap<int, int> map;
#endif
        
        private GraphResolverJob job;
        private JobHandle handle;

        private Graph graph;

        public GraphResolver(IAction[] actions, IActionKeyResolver keyResolver)
        {
            this.graph = new GraphBuilder(keyResolver).Build(actions);
            
            this.indexList = this.graph.AllNodes.ToList();
            this.actionIndexList = this.indexList.Select(x => x.Action).ToList();
            
#if UNITY_COLLECTIONS_2_1
            var map = new NativeParallelMultiHashMap<int, int>(this.indexList.Count, Allocator.Persistent);
#else
            var map = new NativeMultiHashMap<int, int>(this.indexList.Count, Allocator.Persistent);
#endif
            
            for (var i = 0; i < this.indexList.Count; i++)
            {
                var connections = this.indexList[i].Conditions
                    .SelectMany(x => x.Connections.Select(y => this.indexList.IndexOf(y)));

                foreach (var connection in connections)
                {
                    map.Add(i, connection);
                }
            }

            this.map = map;
        }

        public IResolveHandle StartResolve(RunData runData)
        {
            return new ResolveHandle(this, this.map, runData);
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
        
        public Graph GetGraph()
        {
            return this.graph;
        }
        
        public int GetIndex(IAction action) => this.actionIndexList.IndexOf(action);
        public IAction GetAction(int index) => this.actionIndexList[index];
        
        public void Dispose()
        {
            this.map.Dispose();
        }
    }
}