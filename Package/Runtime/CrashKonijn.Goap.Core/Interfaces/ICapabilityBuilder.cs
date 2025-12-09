using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Core
{
    public interface ICapabilityBuilder
    {
        /// <summary>
        ///     Adds an action to the capability.
        /// </summary>
        /// <typeparam name="TAction">The type of the action.</typeparam>
        /// <returns>An instance of <see cref="IActionBuilder{TAction}" />.</returns>
        IActionBuilder<TAction> AddAction<TAction>()
            where TAction : IAction;

        /// <summary>
        ///     Adds a goal to the capability.
        /// </summary>
        /// <typeparam name="TGoal">The type of the goal.</typeparam>
        /// <returns>An instance of <see cref="IGoalBuilder{TGoal}" />.</returns>
        IGoalBuilder<TGoal> AddGoal<TGoal>()
            where TGoal : IGoal;

        /// <summary>
        ///     Adds a world sensor to the capability.
        /// </summary>
        /// <typeparam name="TWorldSensor">The type of the world sensor.</typeparam>
        /// <returns>An instance of <see cref="IWorldSensorBuilder{TWorldSensor}" />.</returns>
        IWorldSensorBuilder<TWorldSensor> AddWorldSensor<TWorldSensor>()
            where TWorldSensor : IWorldSensor;

        /// <summary>
        ///     Adds a target sensor to the capability.
        /// </summary>
        /// <typeparam name="TTargetSensor">The type of the target sensor.</typeparam>
        /// <returns>An instance of <see cref="ITargetSensorBuilder{TTargetSensor}" />.</returns>
        ITargetSensorBuilder<TTargetSensor> AddTargetSensor<TTargetSensor>()
            where TTargetSensor : ITargetSensor;

        /// <summary>
        ///     Adds a multi sensor to the capability.
        /// </summary>
        /// <typeparam name="TMultiSensor">The type of the multi sensor.</typeparam>
        /// <returns>An instance of <see cref="IMultiSensorBuilder{TMultiSensor}" />.</returns>
        IMultiSensorBuilder<TMultiSensor> AddMultiSensor<TMultiSensor>()
            where TMultiSensor : IMultiSensor;

        IWorldKeyBuilder GetWorldKeyBuilder();

        /// <summary>
        ///     Builds the capability configuration.
        /// </summary>
        /// <returns>The built <see cref="ICapabilityConfig" />.</returns>
        ICapabilityConfig Build();
    }
}