using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using Unity.Collections;
using Unity.Mathematics;

namespace CrashKonijn.Goap.Runtime
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

        public void Run(HashSet<IMonoGoapActionProvider> queue)
        {
            this.resolveHandles.Clear();
            
            this.agentType.SensorRunner.Update();
            this.agentType.SensorRunner.SenseGlobal();

            foreach (var agent in queue)
            {
                this.Run(agent);
            }
        }

        private void Run(IMonoGoapActionProvider actionProvider)
        {
            if (actionProvider.IsNull())
                return;
            
            if (actionProvider.CurrentGoal == null)
                return;

            this.agentType.SensorRunner.SenseLocal(actionProvider);

            if (this.IsGoalCompleted(actionProvider))
            {
                var goal = actionProvider.CurrentGoal;
                actionProvider.ClearGoal();
                actionProvider.Events.GoalCompleted(goal);
                return;
            }

            this.FillBuilders(actionProvider);
            
            this.resolveHandles.Add(new JobRunHandle(actionProvider)
            {
                Handle = this.resolver.StartResolve(new RunData
                {
                    StartIndex = new NativeArray<int>(new []{ this.resolver.GetIndex(actionProvider.CurrentGoal) }, Allocator.TempJob),
                    AgentPosition = actionProvider.Position,
                    IsEnabled = new NativeArray<bool>(this.enabledBuilder.Build(), Allocator.TempJob),
                    IsExecutable = new NativeArray<bool>(this.executableBuilder.Build(), Allocator.TempJob),
                    Positions = new NativeArray<float3>(this.positionBuilder.Build(), Allocator.TempJob),
                    Costs = new NativeArray<float>(this.costBuilder.Build(), Allocator.TempJob),
                    ConditionsMet = new NativeArray<bool>(this.conditionBuilder.Build(), Allocator.TempJob),
                    DistanceMultiplier = actionProvider.DistanceMultiplier
                })
            });
        }

        private void FillBuilders(IMonoGoapActionProvider actionProvider)
        {
            var conditionObserver = this.agentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(actionProvider.WorldData);

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
                
                var target = actionProvider.WorldData.GetTarget(node);

                this.executableBuilder.SetExecutable(node, node.IsExecutable(actionProvider.Agent, allMet));
                this.enabledBuilder.SetEnabled(node, node.IsEnabled(actionProvider.Agent));
                this.costBuilder.SetCost(node, node.GetCost(actionProvider.Agent, actionProvider.Agent.Injector));
                
                this.positionBuilder.SetPosition(node, target?.Position);
            }
        }

        private bool IsGoalCompleted(IGoapAgent agent)
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

                if (resolveHandle.ActionProvider.IsNull())
                    continue;
                
                var goal = result.Goal;
                if (goal == null)
                {
                    resolveHandle.ActionProvider.Events.NoActionFound(resolveHandle.ActionProvider.CurrentGoal);
                    continue;
                }
                
                var action = result.Actions.FirstOrDefault() as IGoapAction;
                
                if (action is null)
                {
                    resolveHandle.ActionProvider.Events.NoActionFound(resolveHandle.ActionProvider.CurrentGoal);
                    continue;
                }

                if (action != resolveHandle.ActionProvider.Agent.ActionState.Action)
                {
                    resolveHandle.ActionProvider.SetAction(goal, action, result.Actions);
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
            public IMonoGoapActionProvider ActionProvider { get; }
            public IResolveHandle Handle { get; set; }
            
            public JobRunHandle(IMonoGoapActionProvider actionProvider)
            {
                this.ActionProvider = actionProvider;
            }
        }

        public IGraph GetGraph() => this.resolver.GetGraph();
    }
}