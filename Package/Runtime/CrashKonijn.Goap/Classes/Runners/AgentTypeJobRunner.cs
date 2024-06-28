using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
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
        private readonly IEnabledBuilder enabledBuilder;
        private readonly IPositionBuilder positionBuilder;
        private readonly ICostBuilder costBuilder;
        private readonly IConditionBuilder conditionBuilder;

        public AgentTypeJobRunner(IAgentType agentType, IGraphResolver graphResolver)
        {
            this.agentType = agentType;
            this.resolver = graphResolver;
            
            this.enabledBuilder = this.resolver.GetEnabledBuilder();
            this.executableBuilder = this.resolver.GetExecutableBuilder();
            this.positionBuilder = this.resolver.GetPositionBuilder();
            this.costBuilder = this.resolver.GetCostBuilder();
            this.conditionBuilder = this.resolver.GetConditionBuilder();
        }

        public void Run(HashSet<IMonoAgent> queue)
        {
            this.resolveHandles.Clear();
            
            this.agentType.SensorRunner.Update();
            this.agentType.SensorRunner.SenseGlobal();

            foreach (var agent in queue)
            {
                this.Run(agent);
            }
        }

        private void Run(IMonoAgent agent)
        {
            if (agent.IsNull())
                return;
            
            if (agent.CurrentGoal == null)
                return;

            this.agentType.SensorRunner.SenseLocal(agent);

            if (this.IsGoalCompleted(agent))
            {
                var goal = agent.CurrentGoal;
                agent.ClearGoal();
                agent.Events.GoalCompleted(goal);
                return;
            }

            this.FillBuilders(agent);
            
            this.resolveHandles.Add(new JobRunHandle(agent)
            {
                Handle = this.resolver.StartResolve(new RunData
                {
                    StartIndex = new NativeArray<int>(new []{ this.resolver.GetIndex(agent.CurrentGoal) }, Allocator.Temp),
                    AgentPosition = agent.Position,
                    IsEnabled = new NativeArray<bool>(this.enabledBuilder.Build(), Allocator.TempJob),
                    IsExecutable = new NativeArray<bool>(this.executableBuilder.Build(), Allocator.TempJob),
                    Positions = new NativeArray<float3>(this.positionBuilder.Build(), Allocator.TempJob),
                    Costs = new NativeArray<float>(this.costBuilder.Build(), Allocator.TempJob),
                    ConditionsMet = new NativeArray<bool>(this.conditionBuilder.Build(), Allocator.TempJob),
                    DistanceMultiplier = agent.DistanceMultiplier
                })
            });
        }

        private void FillBuilders(IMonoAgent agent)
        {
            var conditionObserver = this.agentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            this.enabledBuilder.Clear();
            this.executableBuilder.Clear();
            this.positionBuilder.Clear();
            this.conditionBuilder.Clear();

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
                
                var target = agent.WorldData.GetTarget(node);

                this.executableBuilder.SetExecutable(node, node.IsExecutable(agent, allMet));
                this.enabledBuilder.SetEnabled(node, node.IsEnabled(agent));
                this.costBuilder.SetCost(node, node.GetCost(agent, agent.Injector));
                
                this.positionBuilder.SetPosition(node, target?.Position);
            }
        }

        private bool IsGoalCompleted(IMonoAgent agent)
        {
            var conditionObserver = this.agentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);
            
            foreach (var condition in agent.CurrentGoal.Conditions)
            {
                if (!conditionObserver.IsMet(condition))
                    return false;
            }

            return true;
        }

        public void Complete()
        {
            foreach (var resolveHandle in this.resolveHandles)
            {
                var result = resolveHandle.Handle.Complete();

                if (resolveHandle.Agent.IsNull())
                    continue;
                
                var action = result.FirstOrDefault() as IAction;
                
                if (action is null)
                {
                    resolveHandle.Agent.Events.NoActionFound(resolveHandle.Agent.CurrentGoal);
                    continue;
                }

                if (action != resolveHandle.Agent.ActionState.Action)
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