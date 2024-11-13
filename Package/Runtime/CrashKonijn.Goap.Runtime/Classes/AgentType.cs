using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class AgentType : IAgentType
    {
        public string Id { get; }
        public IAgentCollection Agents { get; }
        public IGoapConfig GoapConfig { get; }
        public ISensorRunner SensorRunner { get; }
        public IAgentTypeEvents Events { get; } = new AgentTypeEvents();
        public IGlobalWorldData WorldData { get; }

        private List<IGoal> goals;
        private Dictionary<Type, IGoal> goalsLookup;
        private List<IGoapAction> actions;

        public AgentType(string id, IGoapConfig config, List<IGoal> goals, List<IGoapAction> actions, ISensorRunner sensorRunner, IGlobalWorldData worldData)
        {
            this.Id = id;
            this.GoapConfig = config;
            this.SensorRunner = sensorRunner;
            this.goals = goals;
            this.goalsLookup = goals.ToDictionary(x => x.GetType());
            this.actions = actions;
            this.WorldData = worldData;

            this.Agents = new AgentCollection(this);
        }

        public void Register(IMonoGoapActionProvider actionProvider)
        {
            this.Agents.Add(actionProvider);
        }

        public void Unregister(IMonoGoapActionProvider actionProvider)
        {
            this.Agents.Remove(actionProvider);
        }

        public TGoal ResolveGoal<TGoal>()
            where TGoal : IGoal
        {
            if (this.goalsLookup.TryGetValue(typeof(TGoal), out var result))
                return (TGoal) result;

            throw new KeyNotFoundException($"No action found of type {typeof(TGoal)}");
        }

        public bool AllConditionsMet(IGoapActionProvider actionProvider, IGoapAction action)
        {
            var conditionObserver = this.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(actionProvider.WorldData);

            foreach (var condition in action.Conditions)
            {
                if (!conditionObserver.IsMet(condition))
                    return false;
            }

            return true;
        }

        public List<IConnectable> GetAllNodes() => this.actions.Cast<IConnectable>().Concat(this.goals).ToList();
        public List<IGoapAction> GetActions() => this.actions;
        public List<TAction> GetActions<TAction>() where TAction : IGoapAction => this.actions.OfType<TAction>().ToList();
        public List<IGoal> GetGoals() => this.goals;
    }
}
