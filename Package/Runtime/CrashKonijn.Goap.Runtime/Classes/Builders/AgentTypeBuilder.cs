using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class AgentTypeBuilder
    {
        private readonly AgentTypeConfig agentTypeConfig;

        private readonly List<CapabilityBuilder> capabilityBuilders = new();
        private readonly List<ICapabilityConfig> capabilityConfigs = new();

        public AgentTypeBuilder(string name)
        {
            this.agentTypeConfig = new AgentTypeConfig(name);
        }

        public CapabilityBuilder CreateCapability(string name)
        {
            var capabilityBuilder = new CapabilityBuilder(name);

            this.capabilityBuilders.Add(capabilityBuilder);

            return capabilityBuilder;
        }

        public void CreateCapability(string name, Action<CapabilityBuilder> callback)
        {
            var capabilityBuilder = new CapabilityBuilder(name);

            callback(capabilityBuilder);

            this.capabilityBuilders.Add(capabilityBuilder);
        }

        public void AddCapability<TCapability>()
            where TCapability : CapabilityFactoryBase, new()
        {
            this.capabilityConfigs.Add(new TCapability().Create());
        }

        public void AddCapability(CapabilityFactoryBase capabilityFactory)
        {
            this.capabilityConfigs.Add(capabilityFactory.Create());
        }

        public void AddCapability(MonoCapabilityFactoryBase capabilityFactory)
        {
            this.capabilityConfigs.Add(capabilityFactory.Create());
        }

        public void AddCapability(ScriptableCapabilityFactoryBase capabilityFactory)
        {
            this.capabilityConfigs.Add(capabilityFactory.Create());
        }

        public void AddCapability(CapabilityBuilder capabilityBuilder)
        {
            this.capabilityConfigs.Add(capabilityBuilder.Build());
        }

        public void AddCapability(ICapabilityConfig capabilityConfig)
        {
            this.capabilityConfigs.Add(capabilityConfig);
        }

        public AgentTypeConfig Build()
        {
            this.capabilityConfigs.AddRange(this.capabilityBuilders.Select(x => x.Build()));

            this.agentTypeConfig.Actions = this.capabilityConfigs.SelectMany(x => x.Actions).ToList();
            this.agentTypeConfig.Goals = this.capabilityConfigs.SelectMany(x => x.Goals).ToList();
            this.agentTypeConfig.TargetSensors = this.capabilityConfigs.SelectMany(x => x.TargetSensors).ToList();
            this.agentTypeConfig.WorldSensors = this.capabilityConfigs.SelectMany(x => x.WorldSensors).ToList();
            this.agentTypeConfig.MultiSensors = this.capabilityConfigs.SelectMany(x => x.MultiSensors).ToList();

            return this.agentTypeConfig;
        }
    }

    public class CapabilityBuilder
    {
        private readonly CapabilityConfig capabilityConfig;
        private readonly List<ActionBuilder> actionBuilders = new();
        private readonly List<GoalBuilder> goalBuilders = new();
        private readonly List<TargetSensorBuilder> targetSensorBuilders = new();
        private readonly List<WorldSensorBuilder> worldSensorBuilders = new();
        private readonly List<MultiSensorBuilder> multiSensorBuilders = new();
        private readonly WorldKeyBuilder worldKeyBuilder = new();
        private readonly TargetKeyBuilder targetKeyBuilder = new();

        public CapabilityBuilder(string name)
        {
            this.capabilityConfig = new CapabilityConfig(name);
        }

        public ActionBuilder<TAction> AddAction<TAction>()
            where TAction : IAction
        {
            var actionBuilder = ActionBuilder.Create<TAction>(this.worldKeyBuilder, this.targetKeyBuilder);

            this.actionBuilders.Add(actionBuilder);

            return actionBuilder;
        }

        public GoalBuilder<TGoal> AddGoal<TGoal>()
            where TGoal : IGoal
        {
            var goalBuilder = GoalBuilder.Create<TGoal>(this.worldKeyBuilder);

            this.goalBuilders.Add(goalBuilder);

            return goalBuilder;
        }

        public WorldSensorBuilder<TWorldSensor> AddWorldSensor<TWorldSensor>()
            where TWorldSensor : IWorldSensor
        {
            var worldSensorBuilder = WorldSensorBuilder.Create<TWorldSensor>(this.worldKeyBuilder);

            this.worldSensorBuilders.Add(worldSensorBuilder);

            return worldSensorBuilder;
        }

        public TargetSensorBuilder<TTargetSensor> AddTargetSensor<TTargetSensor>()
            where TTargetSensor : ITargetSensor
        {
            var targetSensorBuilder = TargetSensorBuilder.Create<TTargetSensor>(this.targetKeyBuilder);

            this.targetSensorBuilders.Add(targetSensorBuilder);

            return targetSensorBuilder;
        }

        public MultiSensorBuilder<TMultiSensor> AddMultiSensor<TMultiSensor>()
            where TMultiSensor : IMultiSensor
        {
            var multiSensorBuilder = MultiSensorBuilder.Create<TMultiSensor>();

            this.multiSensorBuilders.Add(multiSensorBuilder);

            return multiSensorBuilder;
        }

        public WorldKeyBuilder GetWorldKeyBuilder()
        {
            return this.worldKeyBuilder;
        }

        public CapabilityConfig Build()
        {
            this.capabilityConfig.Actions = this.actionBuilders.Select(x => x.Build()).ToList();
            this.capabilityConfig.Goals = this.goalBuilders.Select(x => x.Build()).ToList();
            this.capabilityConfig.TargetSensors = this.targetSensorBuilders.Select(x => x.Build()).ToList();
            this.capabilityConfig.WorldSensors = this.worldSensorBuilders.Select(x => x.Build()).ToList();
            this.capabilityConfig.MultiSensors = this.multiSensorBuilders.Select(x => x.Build()).ToList();

            return this.capabilityConfig;
        }
    }
}
