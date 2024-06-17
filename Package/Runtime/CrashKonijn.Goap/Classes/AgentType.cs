using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
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
        private List<IAction> actions;
        private Dictionary<Type, IAction> actionsLookup;

        public AgentType(string id, IGoapConfig config, List<IGoal> goals, List<IAction> actions, ISensorRunner sensorRunner, IGlobalWorldData worldData)
        {
            this.Id = id;
            this.GoapConfig = config;
            this.SensorRunner = sensorRunner;
            this.goals = goals;
            this.goalsLookup = goals.ToDictionary(x => x.GetType());
            this.actions = actions;
            this.actionsLookup = actions.ToDictionary(x => x.GetType());
            this.WorldData = worldData;
            
            this.Agents = new AgentCollection(this);
        }

        public void Register(IMonoAgent agent)
        {
            this.Agents.Add(agent);
        }

        public void Unregister(IMonoAgent agent)
        {
            this.Agents.Remove(agent);
        }

        public TAction ResolveAction<TAction>()
            where TAction : IAction
        {
            if (this.actionsLookup.TryGetValue(typeof(TAction), out var result))
                return (TAction) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TAction)}");
        }

        public TGoal ResolveGoal<TGoal>()
            where TGoal : IGoal
        {
            if (this.goalsLookup.TryGetValue(typeof(TGoal), out var result))
                return (TGoal) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TGoal)}");
        }

        public List<IConnectable> GetAllNodes() => this.actions.Cast<IConnectable>().Concat(this.goals).ToList();
        public List<IAction> GetActions() => this.actions;
        public List<IGoal> GetGoals() => this.goals;
    }
}