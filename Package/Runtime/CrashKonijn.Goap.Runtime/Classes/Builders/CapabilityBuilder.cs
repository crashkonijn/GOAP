using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class CapabilityBuilder : ICapabilityBuilder
    {
        private readonly List<ActionBuilder> actionBuilders = new();
        private readonly CapabilityConfig capabilityConfig;
        private readonly List<GoalBuilder> goalBuilders = new();
        private readonly List<MultiSensorBuilder> multiSensorBuilders = new();
        private readonly TargetKeyBuilder targetKeyBuilder = new();
        private readonly List<TargetSensorBuilder> targetSensorBuilders = new();
        private readonly WorldKeyBuilder worldKeyBuilder = new();
        private readonly List<WorldSensorBuilder> worldSensorBuilders = new();

        public CapabilityBuilder(string name)
        {
            this.capabilityConfig = new CapabilityConfig(name);
        }

        /// <summary>
        ///     Adds an action to the capability.
        /// </summary>
        /// <typeparam name="TAction">The type of the action.</typeparam>
        /// <returns>An instance of <see cref="ActionBuilder{TAction}" />.</returns>
        public IActionBuilder<TAction> AddAction<TAction>()
            where TAction : IAction
        {
            var actionBuilder = ActionBuilder.Create<TAction>(this.worldKeyBuilder, this.targetKeyBuilder);

            this.actionBuilders.Add(actionBuilder);

            return actionBuilder;
        }

        /// <summary>
        ///     Adds a goal to the capability.
        /// </summary>
        /// <typeparam name="TGoal">The type of the goal.</typeparam>
        /// <returns>An instance of <see cref="GoalBuilder{TGoal}" />.</returns>
        public IGoalBuilder<TGoal> AddGoal<TGoal>()
            where TGoal : IGoal
        {
            var goalBuilder = GoalBuilder.Create<TGoal>(this.worldKeyBuilder);

            this.goalBuilders.Add(goalBuilder);

            return goalBuilder;
        }

        /// <summary>
        ///     Adds a world sensor to the capability.
        /// </summary>
        /// <typeparam name="TWorldSensor">The type of the world sensor.</typeparam>
        /// <returns>An instance of <see cref="WorldSensorBuilder{TWorldSensor}" />.</returns>
        public IWorldSensorBuilder<TWorldSensor> AddWorldSensor<TWorldSensor>()
            where TWorldSensor : IWorldSensor
        {
            var worldSensorBuilder = WorldSensorBuilder.Create<TWorldSensor>(this.worldKeyBuilder);

            this.worldSensorBuilders.Add(worldSensorBuilder);

            return worldSensorBuilder;
        }

        /// <summary>
        ///     Adds a target sensor to the capability.
        /// </summary>
        /// <typeparam name="TTargetSensor">The type of the target sensor.</typeparam>
        /// <returns>An instance of <see cref="TargetSensorBuilder{TTargetSensor}" />.</returns>
        public ITargetSensorBuilder<TTargetSensor> AddTargetSensor<TTargetSensor>()
            where TTargetSensor : ITargetSensor
        {
            var targetSensorBuilder = TargetSensorBuilder.Create<TTargetSensor>(this.targetKeyBuilder);

            this.targetSensorBuilders.Add(targetSensorBuilder);

            return targetSensorBuilder;
        }

        /// <summary>
        ///     Adds a multi sensor to the capability.
        /// </summary>
        /// <typeparam name="TMultiSensor">The type of the multi sensor.</typeparam>
        /// <returns>An instance of <see cref="MultiSensorBuilder{TMultiSensor}" />.</returns>
        public IMultiSensorBuilder<TMultiSensor> AddMultiSensor<TMultiSensor>()
            where TMultiSensor : IMultiSensor
        {
            var multiSensorBuilder = MultiSensorBuilder.Create<TMultiSensor>();

            this.multiSensorBuilders.Add(multiSensorBuilder);

            return multiSensorBuilder;
        }

        public IWorldKeyBuilder GetWorldKeyBuilder()
        {
            return this.worldKeyBuilder;
        }

        /// <summary>
        ///     Builds the capability configuration.
        /// </summary>
        /// <returns>The built <see cref="CapabilityConfig" />.</returns>
        public ICapabilityConfig Build()
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