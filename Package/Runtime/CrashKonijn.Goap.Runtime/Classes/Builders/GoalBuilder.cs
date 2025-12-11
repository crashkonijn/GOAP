using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoalBuilder<T> : GoalBuilder, IGoalBuilder<T> where T : IGoal
    {
        public GoalBuilder(WorldKeyBuilder worldKeyBuilder) : base(typeof(T), worldKeyBuilder)
        {
        }

        /// <summary>
        ///     Sets the base cost for the goal.
        /// </summary>
        /// <param name="baseCost">The base cost.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        public IGoalBuilder<T> SetBaseCost(float baseCost)
        {
            this.config.BaseCost = baseCost;
            return this;
        }

        /// <summary>
        ///     Adds a value condition to the goal.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <param name="amount">The amount for the condition.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        public IGoalBuilder<T> AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            this.conditions.Add(new ValueCondition(this.worldKeyBuilder.GetKey<TWorldKey>(), comparison, amount));
            return this;
        }

        /// <summary>
        ///     Adds a reference condition to the goal.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <typeparam name="TValueKey">The type of the value world key reference.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        public GoalBuilder<T> AddCondition<TWorldKey, TValueKey>(Comparison comparison)
            where TWorldKey : IWorldKey
            where TValueKey : IWorldKey
        {
            this.conditions.Add(new ReferenceCondition(this.worldKeyBuilder.GetKey<TWorldKey>(), comparison, this.worldKeyBuilder.GetKey<TValueKey>()));
            return this;
        }

        /// <summary>
        ///     Sets the callback for the goal. This will be called when the goal is created.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        public IGoalBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = obj => callback((T)obj);
            return this;
        }
    }

    public class GoalBuilder
    {
        protected readonly List<ICondition> conditions = new();
        protected readonly GoalConfig config;
        protected readonly WorldKeyBuilder worldKeyBuilder;

        public GoalBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new GoalConfig(type)
            {
                BaseCost = 1,
                ClassType = type.AssemblyQualifiedName
            };
        }

        /// <summary>
        ///     Builds the goal configuration.
        /// </summary>
        /// <returns>The built <see cref="IGoalConfig" />.</returns>
        public IGoalConfig Build()
        {
            this.config.Conditions = this.conditions;
            return this.config;
        }

        public static GoalBuilder<TGoal> Create<TGoal>(WorldKeyBuilder worldKeyBuilder)
            where TGoal : IGoal
        {
            return new GoalBuilder<TGoal>(worldKeyBuilder);
        }
    }
}