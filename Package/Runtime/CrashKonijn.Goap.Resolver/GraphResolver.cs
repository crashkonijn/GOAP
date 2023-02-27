using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Resolver;
using LamosInteractive.Goap;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;
using Unity.Collections;
using Unity.Jobs;
using GraphResolver = CrashKonijn.Goap.Resolver.GraphResolver;

namespace CrashKonijn.Goap.Resolver
{
    public class GraphResolver
    {
        private readonly List<Node> indexList;
        private readonly List<IAction> actionIndexList;
        private NativeMultiHashMap<int, int> map;
        
        private GraphResolverJob job;
        private JobHandle handle;

        public GraphResolver(IAction[] actions, IActionKeyResolver keyResolver)
        {
            var graph = new GraphBuilder(keyResolver).Build(actions);
            
            this.indexList = graph.AllNodes.ToList();
            this.actionIndexList = this.indexList.Select(x => x.Action).ToList();

            var map = new NativeMultiHashMap<int, int>(this.indexList.Count, Allocator.Persistent);
            
            for (var i = 0; i < this.indexList.Count; i++)
            {
                var connections = indexList[i].Conditions
                    .SelectMany(x => x.Connections.Select(y => this.indexList.IndexOf(y)));

                foreach (var connection in connections)
                {
                    map.Add(i, connection);
                }
            }

            this.map = map;
        }

        public ResolveHandle StartResolve(RunData runData)
        {
            return new ResolveHandle(this, this.map, runData);
        }
        
        public ExecutableBuilder GetExecutableBuilder()
        {
            return new ExecutableBuilder(this.actionIndexList);
        }
        
        public int GetIndex(IAction action) => this.actionIndexList.IndexOf(action);
        public IAction GetAction(int index) => this.actionIndexList[index];
        
        public void Dispose()
        {
            this.map.Dispose();
        }
    }
}

public class ResolveHandle
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
        
        // this.job.Execute();
        
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

        return results.ToArray();
    }
}