using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class ActionBuilder
    {
        private readonly ActionConfig config;
        private readonly List<ICondition> conditions = new();
        private readonly List<IEffect> effects = new();
        private readonly WorldKeyBuilder worldKeyBuilder;
        private readonly TargetKeyBuilder targetKeyBuilder;

        public ActionBuilder(Type actionType, WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.targetKeyBuilder = targetKeyBuilder;
            
            this.config = new ActionConfig
            {
                Name = actionType.Name,
                ClassType = actionType.AssemblyQualifiedName,
                BaseCost = 1,
                InRange = 0.5f
            };
        }

        public ActionBuilder SetTarget<TTargetKey>()
            where TTargetKey : ITargetKey
        {
            this.config.Target = this.targetKeyBuilder.GetKey<TTargetKey>();
            return this;
        }

        public ActionBuilder SetBaseCost(int baseCost)
        {
            this.config.BaseCost = baseCost;
            return this;
        }
        
        public ActionBuilder SetInRange(float inRange)
        {
            this.config.InRange = inRange;
            return this;
        }
        
        public ActionBuilder SetMoveMode(ActionMoveMode moveMode)
        {
            this.config.MoveMode = moveMode;
            return this;
        }

        public ActionBuilder AddCondition<TWorldKey>(Comparison comparison, int amount)
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

        public ActionBuilder AddEffect<TWorldKey>(bool increase)
            where TWorldKey : IWorldKey
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = increase
            });
            
            return this;
        }

        public IActionConfig Build()
        {
            this.config.Conditions = this.conditions.ToArray();
            this.config.Effects = this.effects.ToArray();
            
            return this.config;
        }
        
        public static ActionBuilder Create<TAction>(WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
            where TAction : IActionBase
        {
            return new ActionBuilder(typeof(TAction), worldKeyBuilder, targetKeyBuilder);
        }
    }
}