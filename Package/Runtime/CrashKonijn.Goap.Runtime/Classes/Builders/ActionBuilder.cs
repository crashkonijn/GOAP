using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionBuilder<T> : ActionBuilder, IActionBuilder<T> where T : IAction
    {
        public ActionBuilder(WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder) : base(typeof(T), worldKeyBuilder, targetKeyBuilder)
        {
        }

        /// <summary>
        ///     Sets the target key for the action.
        /// </summary>
        /// <typeparam name="TTargetKey">The type of the target key.</typeparam>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetTarget<TTargetKey>()
            where TTargetKey : ITargetKey
        {
            this.config.Target = this.targetKeyBuilder.GetKey<TTargetKey>();
            return this;
        }

        /// <summary>
        ///     Sets the base cost for the action.
        /// </summary>
        /// <param name="baseCost">The base cost.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetBaseCost(float baseCost)
        {
            this.config.BaseCost = baseCost;
            return this;
        }

        /// <summary>
        ///     Sets whether the target should be validated when running the action.
        /// </summary>
        /// <param name="validate">True if the target should be validated; otherwise, false.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetValidateTarget(bool validate)
        {
            this.config.ValidateTarget = validate;
            return this;
        }

        /// <summary>
        ///     Sets whether the action requires a target.
        /// </summary>
        /// <param name="requiresTarget">True if the action requires a target; otherwise, false.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetRequiresTarget(bool requiresTarget)
        {
            this.config.RequiresTarget = requiresTarget;
            return this;
        }

        /// <summary>
        ///     Sets whether the conditions should be validated when running the action.
        /// </summary>
        /// <param name="validate">True if the conditions should be validated; otherwise, false.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetValidateConditions(bool validate)
        {
            this.config.ValidateConditions = validate;
            return this;
        }

        /// <summary>
        ///     Sets the stopping distance for the action. This is the distance at which the action will stop moving towards the
        ///     target.
        /// </summary>
        /// <param name="inRange">The stopping distance.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetStoppingDistance(float inRange)
        {
            this.config.StoppingDistance = inRange;
            return this;
        }

        [Obsolete("Use `SetStoppingDistance(float inRange)` instead.")]
        public IActionBuilder<T> SetInRange(float inRange)
        {
            this.config.StoppingDistance = inRange;
            return this;
        }

        /// <summary>
        ///     Sets the move mode for the action.
        /// </summary>
        /// <param name="moveMode">The move mode.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetMoveMode(ActionMoveMode moveMode)
        {
            this.config.MoveMode = moveMode;
            return this;
        }

        /// <summary>
        ///     Adds a condition to the action.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <param name="amount">The amount for the condition.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            this.conditions.Add(new ValueCondition
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Comparison = comparison,
                Amount = amount
            });

            return this;
        }
        
        /// <summary>
        ///     Adds a reference condition to the action.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <typeparam name="TValueKey">The type of the value world key reference.</typeparam>
        /// <param name="comparison">The comparison type.</param>
        /// <returns>The current instance of <see cref="GoalBuilder{T}" />.</returns>
        public IActionBuilder<T> AddCondition<TWorldKey, TValueKey>(Comparison comparison)
            where TWorldKey : IWorldKey
            where TValueKey : IWorldKey
        {
            this.conditions.Add(new ReferenceCondition(this.worldKeyBuilder.GetKey<TWorldKey>(), comparison, this.worldKeyBuilder.GetKey<TValueKey>()));
            return this;
        }

        [Obsolete("Use `AddEffect<TWorldKey>(EffectType type)` instead.")]
        public IActionBuilder<T> AddEffect<TWorldKey>(bool increase)
            where TWorldKey : IWorldKey
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = increase
            });

            return this;
        }

        /// <summary>
        ///     Adds an effect to the action.
        /// </summary>
        /// <typeparam name="TWorldKey">The type of the world key.</typeparam>
        /// <param name="type">The effect type.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> AddEffect<TWorldKey>(EffectType type)
            where TWorldKey : IWorldKey
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = type == EffectType.Increase
            });

            return this;
        }

        /// <summary>
        ///     Sets the properties for the action.
        /// </summary>
        /// <param name="properties">The action properties.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetProperties(IActionProperties properties)
        {
            this.ValidateProperties(properties);

            this.config.Properties = properties;
            return this;
        }

        /// <summary>
        ///     Sets the callback for when the action is created. This can be used to set up the action with custom data.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance of <see cref="ActionBuilder{T}" />.</returns>
        public IActionBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = obj => callback((T)obj);
            return this;
        }
    }

    public class ActionBuilder
    {
        protected readonly Type actionType;
        protected readonly List<ICondition> conditions = new();
        protected readonly ActionConfig config;
        protected readonly List<IEffect> effects = new();
        protected readonly TargetKeyBuilder targetKeyBuilder;
        protected readonly WorldKeyBuilder worldKeyBuilder;

        public ActionBuilder(Type actionType, WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
        {
            this.actionType = actionType;
            this.worldKeyBuilder = worldKeyBuilder;
            this.targetKeyBuilder = targetKeyBuilder;

            var propType = this.actionType.GetPropertiesType();

            this.config = new ActionConfig
            {
                Name = actionType.Name,
                ClassType = actionType.AssemblyQualifiedName,
                BaseCost = 1,
                StoppingDistance = 0.5f,
                RequiresTarget = true,
                ValidateConditions = true,
                ValidateTarget = true,
                Properties = (IActionProperties)Activator.CreateInstance(propType)
            };
        }

        protected void ValidateProperties(IActionProperties properties)
        {
            var actionPropsType = this.actionType.GetPropertiesType();

            if (actionPropsType == properties.GetType())
                return;

            throw new ArgumentException($"The provided properties do not match the expected type '{actionPropsType.Name}'.", nameof(properties));
        }

        public IActionConfig Build()
        {
            this.config.Conditions = this.conditions.ToArray();
            this.config.Effects = this.effects.ToArray();

            return this.config;
        }

        public static ActionBuilder<TAction> Create<TAction>(WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
            where TAction : IAction
        {
            return new ActionBuilder<TAction>(worldKeyBuilder, targetKeyBuilder);
        }
    }
}