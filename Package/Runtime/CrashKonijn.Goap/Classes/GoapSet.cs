using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class GoapSet : IGoapSet
    {
        public string Id { get; }
        public IAgentCollection Agents { get; } = new AgentCollection();
        public IGoapConfig GoapConfig { get; }
        public ISensorRunner SensorRunner { get; }
        public IAgentDebugger Debugger { get; }

        private List<IGoalBase> goals;
        private List<IActionBase> actions;

        public GoapSet(string id, IGoapConfig config, List<IGoalBase> goals, List<IActionBase> actions, ISensorRunner sensorRunner, IAgentDebugger debugger)
        {
            this.Id = id;
            this.GoapConfig = config;
            this.SensorRunner = sensorRunner;
            this.goals = goals;
            this.actions = actions;
            this.Debugger = debugger;
        }

        public void Register(IMonoAgent agent) => this.Agents.Add(agent);
        public void Unregister(IMonoAgent agent) => this.Agents.Remove(agent);

        public TAction ResolveAction<TAction>()
            where TAction : ActionBase
        {
            var result = this.actions.FirstOrDefault(x => x.GetType() == typeof(TAction));

            if (result != null)
                return (TAction) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TAction)}");
        }

        public TGoal ResolveGoal<TGoal>()
            where TGoal : IGoalBase
        {
            var result = this.goals.FirstOrDefault(x => x.GetType() == typeof(TGoal));

            if (result != null)
                return (TGoal) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TGoal)}");
        }

        public List<IAction> GetAllNodes() => this.actions.Cast<IAction>().Concat(this.goals).ToList();
        public List<IActionBase> GetActions() => this.actions;
        public List<IGoalBase> GetGoals() => this.goals;

        public AgentDebugGraph GetDebugGraph()
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