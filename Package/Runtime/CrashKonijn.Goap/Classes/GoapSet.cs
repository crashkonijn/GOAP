using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class GoapSet : IGoapSet
    {
        public IAgentCollection Agents { get; } = new AgentCollection();
        public GoapConfig GoapConfig { get; }
        public SensorRunner SensorRunner { get; }
        
        private List<IGoalBase> goals;
        private List<IActionBase> actions;

        public GoapSet(GoapConfig config, List<IGoalBase> goals, List<IActionBase> actions, SensorRunner sensorRunner)
        {
            this.GoapConfig = config;
            this.SensorRunner = sensorRunner;
            this.goals = goals;
            this.actions = actions;
        }

        public void Register(AgentBehaviour agent) => this.Agents.Add(agent);
        public void Unregister(AgentBehaviour agent) => this.Agents.Remove(agent);

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

        public List<IAction> GetAllNodes()
        {
            return this.actions.Cast<IAction>().Concat(this.goals.Cast<IAction>()).ToList();
        }

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