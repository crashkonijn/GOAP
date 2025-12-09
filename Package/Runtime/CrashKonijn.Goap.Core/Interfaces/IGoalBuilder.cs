using System;

namespace CrashKonijn.Goap.Core
{
    public interface IGoalBuilder<T> where T : IGoal
    {
        /// <summary>
        ///     Sets the base cost for the goal.
        /// </summary>
        /// <param name="baseCost">The base cost.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        IGoalBuilder<T> SetBaseCost(float baseCost);

        /// <summary>
        ///     Adds a condition to the goal.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <param name="amount">The amount for the condition.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        IGoalBuilder<T> AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey;

        /// <summary>
        ///     Sets the callback for the goal. This will be called when the goal is created.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        IGoalBuilder<T> SetCallback(Action<T> callback);

        /// <summary>
        ///     Builds the goal configuration.
        /// </summary>
        /// <returns>The built <see cref="IGoalConfig" />.</returns>
        IGoalConfig Build();
    }
}