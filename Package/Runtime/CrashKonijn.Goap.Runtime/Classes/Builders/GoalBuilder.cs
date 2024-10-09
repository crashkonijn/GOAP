using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoalBuilder<T> : GoalBuilder
        where T : IGoal
    {
        public GoalBuilder(WorldKeyBuilder worldKeyBuilder) : base(typeof(T), worldKeyBuilder) { }

        public GoalBuilder<T> SetBaseCost(float baseCost)
        {
            this.config.BaseCost = baseCost;
            return this;
        }

        public GoalBuilder<T> AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            this.conditions.Add(new Condition(this.worldKeyBuilder.GetKey<TWorldKey>(), comparison, amount));
            return this;
        }

        public GoalBuilder<T> SetCallback(Action<T> callback)
        {
            this.config.Callback = (obj) => callback((T) obj);
            return this;
        }
    }

    public class GoalBuilder
    {
        protected readonly GoalConfig config;
        protected readonly List<ICondition> conditions = new();
        protected readonly WorldKeyBuilder worldKeyBuilder;

        public GoalBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new GoalConfig(type)
            {
                BaseCost = 1,
                ClassType = type.AssemblyQualifiedName,
            };
        }

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
