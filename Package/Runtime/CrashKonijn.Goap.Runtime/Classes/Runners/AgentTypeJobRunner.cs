using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
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

        private List<int> goalIndexes = new();

        public AgentTypeJobRunner(IAgentType agentType, IGraphResolver graphResolver)
        {
            this.agentType = agentType;
            this.resolver = graphResolver;

            this.enabledBuilder = this.resolver.GetEnabledBuilder();
            this.executableBuilder = this.resolver.GetExecutableBuilder();
            this.positionBuilder = this.resolver.GetPositionBuilder();
            this.costBuilder = this.resolver.GetCostBuilder();
            this.conditionBuilder = this.resolver.GetConditionBuilder();

            agentType.SensorRunner.InitializeGraph(graphResolver.GetGraph());
        }

        public void Run(IMonoGoapActionProvider[] queue)
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

            if (actionProvider.AgentType != this.agentType)
                return;

            if (this.IsGoalCompleted(actionProvider))
            {
                var goal = actionProvider.CurrentPlan;
                actionProvider.ClearGoal();
                actionProvider.Events.GoalCompleted(goal.Goal);
            }

            if (!this.MayResolve(actionProvider))
                return;

            var goalRequest = actionProvider.GoalRequest;

            if (goalRequest == null)
                return;

            this.agentType.SensorRunner.SenseLocal(actionProvider, goalRequest);

            this.FillBuilders(actionProvider);

            this.LogRequest(actionProvider, goalRequest);

            this.goalIndexes.Clear();

            foreach (var goal in goalRequest.Goals)
            {
                if (this.IsGoalCompleted(actionProvider, goal))
                    continue;

                this.goalIndexes.Add(this.resolver.GetIndex(goal));
            }

            this.resolveHandles.Add(new JobRunHandle(actionProvider, goalRequest)
            {
                Handle = this.resolver.StartResolve(new RunData
                {
                    StartIndex = new NativeArray<int>(this.goalIndexes.ToArray(), Allocator.TempJob),
                    AgentPosition = actionProvider.Position,
                    IsEnabled = new NativeArray<bool>(this.enabledBuilder.Build(), Allocator.TempJob),
                    IsExecutable = new NativeArray<bool>(this.executableBuilder.Build(), Allocator.TempJob),
                    Positions = new NativeArray<float3>(this.positionBuilder.Build(), Allocator.TempJob),
                    Costs = new NativeArray<float>(this.costBuilder.Build(), Allocator.TempJob),
                    ConditionsMet = new NativeArray<bool>(this.conditionBuilder.Build(), Allocator.TempJob),
                    DistanceMultiplier = actionProvider.DistanceMultiplier,
                }),
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

            foreach (var goal in this.agentType.GetGoals())
            {
                this.costBuilder.SetCost(goal, goal.GetCost(actionProvider.Receiver, actionProvider.Receiver.Injector));
            }

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

                this.executableBuilder.SetExecutable(node, node.IsExecutable(actionProvider.Receiver, allMet));
                this.enabledBuilder.SetEnabled(node, node.IsEnabled(actionProvider.Receiver));
                this.costBuilder.SetCost(node, node.GetCost(actionProvider.Receiver, actionProvider.Receiver.Injector, target));

                this.positionBuilder.SetPosition(node, target?.Position);
            }
        }

        private bool IsGoalCompleted(IMonoGoapActionProvider actionProvider)
        {
            if (actionProvider.CurrentPlan?.Goal == null)
                return false;

            this.agentType.SensorRunner.SenseLocal(actionProvider, actionProvider.CurrentPlan.Goal);

            return this.IsGoalCompleted(actionProvider, actionProvider.CurrentPlan.Goal);
        }

        private bool IsGoalCompleted(IGoapActionProvider actionProvider, IGoal goal)
        {
            if (goal == null)
                return false;

            var conditionObserver = this.agentType.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(actionProvider.WorldData);

            foreach (var condition in goal.Conditions)
            {
                if (!conditionObserver.IsMet(condition))
                    return false;
            }

            return true;
        }

        private bool MayResolve(IGoapActionProvider actionProvider)
        {
            if (actionProvider.Receiver.ActionState?.RunState == null)
                return true;

            if (actionProvider.Receiver is not IAgent agent)
                return true;

            return actionProvider.Receiver.ActionState.RunState.MayResolve(agent);
        }

        public void Complete()
        {
            foreach (var resolveHandle in this.resolveHandles)
            {
                var result = resolveHandle.Handle.Complete();

                if (resolveHandle.ActionProvider.GoalRequest != resolveHandle.GoalRequest)
                    continue;

                if (resolveHandle.ActionProvider.IsNull())
                    continue;

                var goal = result.Goal;
                if (goal == null)
                {
                    resolveHandle.ActionProvider.Events.NoActionFound(resolveHandle.GoalRequest);
                    continue;
                }

                var action = result.Actions.FirstOrDefault() as IGoapAction;

                if (action is null)
                {
                    resolveHandle.ActionProvider.Events.NoActionFound(resolveHandle.GoalRequest);
                    continue;
                }

                if (action != resolveHandle.ActionProvider.Receiver.ActionState.Action)
                {
                    resolveHandle.ActionProvider.SetAction(new GoalResult
                    {
                        Goal = goal,
                        Plan = result.Actions,
                        Action = action,
                    });
                }
            }

            this.resolveHandles.Clear();
        }

        private void LogRequest(IGoapActionProvider actionProvider, IGoalRequest request)
        {
#if UNITY_EDITOR
            if (actionProvider.Logger == null)
                return;

            if (!actionProvider.Logger.ShouldLog())
                return;

            var builder = new StringBuilder();
            builder.Append("Trying to resolve goals ");

            foreach (var goal in request.Goals)
            {
                builder.Append(goal.GetType().GetGenericTypeName());
                builder.Append(", ");
            }

            actionProvider.Logger.Log(builder.ToString());
#endif
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
            public IGoalRequest GoalRequest { get; set; }

            public JobRunHandle(IMonoGoapActionProvider actionProvider, IGoalRequest goalRequest)
            {
                this.ActionProvider = actionProvider;
                this.GoalRequest = goalRequest;
            }
        }

        public IGraph GetGraph() => this.resolver.GetGraph();
    }
}
