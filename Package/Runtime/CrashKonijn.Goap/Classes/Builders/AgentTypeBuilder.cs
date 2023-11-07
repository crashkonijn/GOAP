using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class AgentTypeBuilder
    {
        private readonly AgentTypeConfig agentTypeConfig;

        private readonly List<ActionBuilder> actionBuilders = new();
        private readonly List<GoalBuilder> goalBuilders = new();
        private readonly List<TargetSensorBuilder> targetSensorBuilders = new();
        private readonly List<WorldSensorBuilder> worldSensorBuilders = new();
        private readonly WorldKeyBuilder worldKeyBuilder = new();
        private readonly TargetKeyBuilder targetKeyBuilder = new();

        public AgentTypeBuilder(string name)
        {
            this.agentTypeConfig = new AgentTypeConfig(name);
        }
        
        public ActionBuilder AddAction<TAction>()
            where TAction : IActionBase
        {
            var actionBuilder = ActionBuilder.Create<TAction>(this.worldKeyBuilder, this.targetKeyBuilder);
            
            this.actionBuilders.Add(actionBuilder);
            
            return actionBuilder;
        }
        
        public GoalBuilder AddGoal<TGoal>()
            where TGoal : IGoalBase
        {
            var goalBuilder = GoalBuilder.Create<TGoal>(this.worldKeyBuilder);

            this.goalBuilders.Add(goalBuilder);
            
            return goalBuilder;
        }
        
        public WorldSensorBuilder AddWorldSensor<TWorldSensor>()
            where TWorldSensor : IWorldSensor
        {
            var worldSensorBuilder = WorldSensorBuilder.Create<TWorldSensor>(this.worldKeyBuilder);

            this.worldSensorBuilders.Add(worldSensorBuilder);
            
            return worldSensorBuilder;
        }
        
        public TargetSensorBuilder AddTargetSensor<TTargetSensor>()
            where TTargetSensor : ITargetSensor
        {
            var targetSensorBuilder = TargetSensorBuilder.Create<TTargetSensor>(this.targetKeyBuilder);

            this.targetSensorBuilders.Add(targetSensorBuilder);
            
            return targetSensorBuilder;
        }

        public WorldKeyBuilder GetWorldKeyBuilder()
        {
            return this.worldKeyBuilder;
        }
        
        public AgentTypeConfig Build()
        {
            this.agentTypeConfig.Actions = this.actionBuilders.Select(x => x.Build()).ToList();
            this.agentTypeConfig.Goals = this.goalBuilders.Select(x => x.Build()).ToList();
            this.agentTypeConfig.TargetSensors = this.targetSensorBuilders.Select(x => x.Build()).ToList();
            this.agentTypeConfig.WorldSensors = this.worldSensorBuilders.Select(x => x.Build()).ToList();
            
            return this.agentTypeConfig;
        }

        public AgentTypeBuilder SetAgentDebugger<TDebugger>()
            where TDebugger : IAgentDebugger
        {
            this.agentTypeConfig.DebuggerClass = typeof(TDebugger).AssemblyQualifiedName;

            return this;
        }
    }
}