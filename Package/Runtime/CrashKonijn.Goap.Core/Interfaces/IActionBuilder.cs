using System;
using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IActionBuilder<T> where T : IAction
    {
        /// <summary>
        ///     Sets the target key for the action.
        /// </summary>
        /// <typeparam name="TTargetKey">The type of the target key.</typeparam>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetTarget<TTargetKey>()
            where TTargetKey : ITargetKey;

        /// <summary>
        ///     Sets the base cost for the action.
        /// </summary>
        /// <param name="baseCost">The base cost.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetBaseCost(float baseCost);

        /// <summary>
        ///     Sets whether the target should be validated when running the action.
        /// </summary>
        /// <param name="validate">True if the target should be validated; otherwise, false.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetValidateTarget(bool validate);

        /// <summary>
        ///     Sets whether the action requires a target.
        /// </summary>
        /// <param name="requiresTarget">True if the action requires a target; otherwise, false.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetRequiresTarget(bool requiresTarget);

        /// <summary>
        ///     Sets whether the conditions should be validated when running the action.
        /// </summary>
        /// <param name="validate">True if the conditions should be validated; otherwise, false.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetValidateConditions(bool validate);

        /// <summary>
        ///     Sets the stopping distance for the action. This is the distance at which the action will stop moving towards the
        ///     target.
        /// </summary>
        /// <param name="inRange">The stopping distance.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetStoppingDistance(float inRange);

        IActionBuilder<T> SetInRange(float inRange);

        /// <summary>
        ///     Sets the move mode for the action.
        /// </summary>
        /// <param name="moveMode">The move mode.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetMoveMode(ActionMoveMode moveMode);

        /// <summary>
        ///     Adds a condition to the action.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <param name="amount">The amount for the condition.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey;

        /// <summary>
        ///     Adds a reference condition to the action.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <typeparam name="TValueKey">The type of the value world key reference.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        IActionBuilder<T> AddCondition<TWorldKey, TValueKey>(Comparison comparison)
            where TWorldKey : IWorldKey
            where TValueKey : IWorldKey;

        IActionBuilder<T> AddEffect<TWorldKey>(bool increase)
            where TWorldKey : IWorldKey;

        /// <summary>
        ///     Adds an effect to the action.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <param name="type">The effect type.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> AddEffect<TWorldKey>(EffectType type)
            where TWorldKey : IWorldKey;

        /// <summary>
        ///     Sets the properties for the action.
        /// </summary>
        /// <param name="properties">The action properties.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetProperties(IActionProperties properties);

        /// <summary>
        ///     Sets the callback for when the action is created. This can be used to set up the action with custom data.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        IActionBuilder<T> SetCallback(Action<T> callback);

        IActionConfig Build();
    }
}