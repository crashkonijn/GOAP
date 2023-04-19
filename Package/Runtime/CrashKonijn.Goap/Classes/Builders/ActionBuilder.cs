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
        
        public ActionBuilder SetTarget(string target)
        {
            this.config.Target = this.targetKeyBuilder.GetKey(target);
            return this;
        }
        
        public ActionBuilder SetTarget<T1>(string target)
        {
            this.config.Target = this.targetKeyBuilder.GetKey<T1>(target);
            return this;
        }
        
        public ActionBuilder SetTarget<T1, T2>(string target)
        {
            this.config.Target = this.targetKeyBuilder.GetKey<T1, T2>(target);
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
        
        public ActionBuilder AddCondition(string key, Comparison comparison, int amount)
        {
            this.conditions.Add(new Condition
            {
                WorldKey = this.worldKeyBuilder.GetKey(key),
                Comparison = comparison,
                Amount = amount,
            });
            
            return this;
        }
        
        public ActionBuilder AddCondition<T>(string key, Comparison comparison, int amount)
        {
            this.conditions.Add(new Condition
            {
                WorldKey = this.worldKeyBuilder.GetKey<T>(key),
                Comparison = comparison,
                Amount = amount,
            });
            
            return this;
        }
        
        public ActionBuilder AddEffect(string key, bool increase)
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey(key),
                Increase = increase
            });
            
            return this;
        }
        
        public ActionBuilder AddEffect<T>(string key, bool increase)
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<T>(key),
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