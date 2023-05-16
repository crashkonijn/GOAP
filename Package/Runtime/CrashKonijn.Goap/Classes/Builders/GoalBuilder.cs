using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class GoalBuilder
    {
        private readonly GoalConfig config;
        private readonly List<ICondition> conditions = new();
        private readonly WorldKeyBuilder worldKeyBuilder;

        public GoalBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new GoalConfig(type)
            {
                BaseCost = 1,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public GoalBuilder SetBaseCost(int baseCost)
        {
            this.config.BaseCost = baseCost;
            return this;
        }
        
        public GoalBuilder AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            this.conditions.Add(new Condition(this.worldKeyBuilder.GetKey<TWorldKey>(), comparison, amount));
            return this;
        }
        
        public IGoalConfig Build()
        {
            this.config.Conditions = this.conditions;
            return this.config;
        }
        
        public static GoalBuilder Create<TGoal>(WorldKeyBuilder worldKeyBuilder)
            where TGoal : IGoalBase
        {
            return new GoalBuilder(typeof(TGoal), worldKeyBuilder);
        }
    }
}