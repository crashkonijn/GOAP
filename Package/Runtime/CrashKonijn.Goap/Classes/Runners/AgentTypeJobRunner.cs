using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
using CrashKonijn.Goap.Resolver.Models;
using Unity.Collections;
using Unity.Mathematics;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class AgentTypeJobRunner : IAgentTypeJobRunner
    {
        private readonly IAgentType agentType;
        private readonly IGraphResolver resolver;
        
        private List<JobRunHandle> resolveHandles = new();
        private readonly IExecutableBuilder executableBuilder;
        private readonly IPositionBuilder positionBuilder;
        private readonly ICostBuilder costBuilder;
        private readonly IConditionBuilder conditionBuilder;

        public AgentTypeJobRunner(IAgentType agentType, IGraphResolver graphResolver)
        {
            this.agentType = agentType;
            this.resolver = graphResolver;
            
            this.executableBuilder = this.resolver.GetExecutableBuilder();
            this.positionBuilder = this.resolver.GetPositionBuilder();
            this.costBuilder = this.resolver.GetCostBuilder();
            this.conditionBuilder = this.resolver.GetConditionBuilder();
        }

        public void Run(HashSet<IMonoAgent> queue)
        {
            this.resolveHandles.Clear();
            
            this.agentType.SensorRunner.Update();
            
            var globalData = this.agentType.SensorRunner.SenseGlobal();

            foreach (var agent in queue)
            {
                this.Run(globalData, agent);
            }
        }

        private void Run(IGlobalWorldData globalData, IMonoAgent agent)
        {
            if (agent.IsNull())
                return;
            
            if (agent.CurrentGoal == null)
                return;

            var localData = this.agentType.SensorRunner.SenseLocal(globalData, agent);

            if (this.IsGoalCompleted(localData, agent))
            {
                var goal = agent.CurrentGoal;
                agent.ClearGoal();
                agent.Events.GoalCompleted(goal);
                return;
            }

            this.FillBuilders(localData, agent);
            
            this.resolveHandles.Add(new JobRunHandle(agent)
            {
                Handle = this.resolver.StartResolve(new RunData
                {
                    StartIndex = this.resolver.GetIndex(agent.CurrentGoal),
                    IsExecutable = new NativeArray<bool>(this.executableBuilder.Build(), Allocator.TempJob),
                    Positions = new NativeArray<float3>(this.positionBuilder.Build(), Allocator.TempJob),
                    Costs = new NativeArray<float>(this.costBuilder.Build(), Allocator.TempJob),
                    ConditionsMet = new NativeArray<bool>(this.conditionBuilder.Build(), Allocator.TempJob),
                    DistanceMultiplier = agent.DistanceMultiplier
                })
            });
        }

        private void FillBuilders(ILocalWorldData localData, IMonoAgent agent)
        {
            var conditionObserver = this.agentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(localData);

            this.executableBuilder.Clear();
            this.positionBuilder.Clear();
            this.conditionBuilder.Clear();

            var transformTarget = new TransformTarget(agent.transform);

            foreach (var node in this.agentType.GetActions())
            {
                var allMet = true;
                
                foreach (var condition in node.Conditions)
                {
                    if (!conditionObserver.IsMet(condition))
                    {
                        allMet = false;
                        continue;
                    }

                    this.conditionBuilder.SetConditionMet(condition, true);
                }
                
                var target = localData.GetTarget(node);

                this.executableBuilder.SetExecutable(node, allMet);
                this.costBuilder.SetCost(node, node.GetCost(agent, agent.Injector));
                
                this.positionBuilder.SetPosition(node, target?.Position ?? transformTarget.Position);
            }
        }

        private bool IsGoalCompleted(ILocalWorldData localData, IMonoAgent agent)
        {
            var conditionObserver = this.agentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(localData);
            
            return agent.CurrentGoal.Conditions.All(x => conditionObserver.IsMet(x));
        }

        public void Complete()
        {
            foreach (var resolveHandle in this.resolveHandles)
            {
                var result = resolveHandle.Handle.Complete().OfType<IAction>().ToList();

                if (resolveHandle.Agent.IsNull())
                    continue;
                
                var action = result.FirstOrDefault();
                
                if (action is null)
                {
                    resolveHandle.Agent.Events.NoActionFound(resolveHandle.Agent.CurrentGoal);
                    continue;
                }

                if (action != resolveHandle.Agent.CurrentAction)
                {
                    resolveHandle.Agent.SetAction(action, result, resolveHandle.Agent.WorldData.GetTarget(action));
                }
            }
            
            this.resolveHandles.Clear();
        }

        public void Dispose()
        {
            foreach (var resolveHandle in this.resolveHandles)
            {
                resolveHandle.Handle.Complete();
            }
            
            this.resolver.Dispose();
        }

        private class JobRunHandle
        {
            public IMonoAgent Agent { get; }
            public IResolveHandle Handle { get; set; }
            
            public JobRunHandle(IMonoAgent agent)
            {
                this.Agent = agent;
            }
        }

        public IGraph GetGraph() => this.resolver.GetGraph();
    }
}