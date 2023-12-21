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
        public IAgentDebugger Debugger { get; }
        public IAgentTypeEvents Events { get; } = new AgentTypeEvents();
        public IGlobalWorldData WorldData { get; }

        private List<IGoal> goals;
        private List<IAction> actions;

        public AgentType(string id, IGoapConfig config, List<IGoal> goals, List<IAction> actions, ISensorRunner sensorRunner, IAgentDebugger debugger, IGlobalWorldData worldData)
        {
            this.Id = id;
            this.GoapConfig = config;
            this.SensorRunner = sensorRunner;
            this.goals = goals;
            this.actions = actions;
            this.Debugger = debugger;
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
            var result = this.actions.FirstOrDefault(x => x.GetType() == typeof(TAction));

            if (result != null)
                return (TAction) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TAction)}");
        }

        public TGoal ResolveGoal<TGoal>()
            where TGoal : IGoal
        {
            var result = this.goals.FirstOrDefault(x => x.GetType() == typeof(TGoal));

            if (result != null)
                return (TGoal) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TGoal)}");
        }

        public List<IConnectable> GetAllNodes() => this.actions.Cast<IConnectable>().Concat(this.goals).ToList();
        public List<IAction> GetActions() => this.actions;
        public List<IGoal> GetGoals() => this.goals;

        public IAgentDebugGraph GetDebugGraph()
        {
            return new AgentDebugGraph
            {
                Goals = this.goals,
                Actions = this.actions,
                Config = this.GoapConfig
            };
        }
    }
}