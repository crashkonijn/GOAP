using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionBuilder<T> : ActionBuilder
        where T : IAction
    {
        public ActionBuilder(WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder) : base(typeof(T), worldKeyBuilder, targetKeyBuilder) { }

        public ActionBuilder<T> SetTarget<TTargetKey>()
            where TTargetKey : ITargetKey
        {
            this.config.Target = this.targetKeyBuilder.GetKey<TTargetKey>();
            return this;
        }

        public ActionBuilder<T> SetBaseCost(float baseCost)
        {
            this.config.BaseCost = baseCost;
            return this;
        }

        public ActionBuilder<T> SetValidateTarget(bool validate)
        {
            this.config.ValidateTarget = validate;
            return this;
        }

        public ActionBuilder<T> SetRequiresTarget(bool requiresTarget)
        {
            this.config.RequiresTarget = requiresTarget;
            return this;
        }

        public ActionBuilder<T> SetValidateConditions(bool validate)
        {
            this.config.ValidateConditions = validate;
            return this;
        }

        public ActionBuilder<T> SetStoppingDistance(float inRange)
        {
            this.config.StoppingDistance = inRange;
            return this;
        }

        [Obsolete("Use `SetStoppingDistance(float inRange)` instead.")]
        public ActionBuilder<T> SetInRange(float inRange)
        {
            this.config.StoppingDistance = inRange;
            return this;
        }

        public ActionBuilder<T> SetMoveMode(ActionMoveMode moveMode)
        {
            this.config.MoveMode = moveMode;
            return this;
        }

        public ActionBuilder<T> AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            this.conditions.Add(new Condition
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Comparison = comparison,
                Amount = amount,
            });

            return this;
        }

        [Obsolete("Use `AddEffect<TWorldKey>(EffectType type)` instead.")]
        public ActionBuilder<T> AddEffect<TWorldKey>(bool increase)
            where TWorldKey : IWorldKey
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = increase,
            });

            return this;
        }

        public ActionBuilder<T> AddEffect<TWorldKey>(EffectType type)
            where TWorldKey : IWorldKey
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = type == EffectType.Increase,
            });

            return this;
        }

        public ActionBuilder<T> SetProperties(IActionProperties properties)
        {
            this.ValidateProperties(properties);

            this.config.Properties = properties;
            return this;
        }

        public ActionBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = (obj) => callback((T) obj);
            return this;
        }
    }

    public class ActionBuilder
    {
        protected readonly ActionConfig config;
        protected readonly List<ICondition> conditions = new();
        protected readonly List<IEffect> effects = new();
        protected readonly WorldKeyBuilder worldKeyBuilder;
        protected readonly TargetKeyBuilder targetKeyBuilder;
        protected readonly Type actionType;

        public ActionBuilder(Type actionType, WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
        {
            this.actionType = actionType;
            this.worldKeyBuilder = worldKeyBuilder;
            this.targetKeyBuilder = targetKeyBuilder;

            var propType = this.GetPropertiesType();

            this.config = new ActionConfig
            {
                Name = actionType.Name,
                ClassType = actionType.AssemblyQualifiedName,
                BaseCost = 1,
                StoppingDistance = 0.5f,
                RequiresTarget = true,
                ValidateConditions = true,
                ValidateTarget = true,
                Properties = (IActionProperties) Activator.CreateInstance(propType),
            };
        }

        protected void ValidateProperties(IActionProperties properties)
        {
            var actionPropsType = this.GetPropertiesType();

            if (actionPropsType == properties.GetType())
                return;

            throw new ArgumentException($"The provided properties do not match the expected type '{actionPropsType.Name}'.", nameof(properties));
        }

        protected Type GetPropertiesType()
        {
            var baseType = this.actionType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(GoapActionBase<,>))
                    return baseType.GetGenericArguments()[1];

                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(GoapActionBase<>))
                    return typeof(EmptyActionProperties);

                baseType = baseType.BaseType;
            }

            return null;
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
