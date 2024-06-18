using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class ActionBuilder
    {
        private readonly ActionConfig config;
        private readonly List<ICondition> conditions = new();
        private readonly List<IEffect> effects = new();
        private readonly WorldKeyBuilder worldKeyBuilder;
        private readonly TargetKeyBuilder targetKeyBuilder;
        private readonly Type actionType;

        public ActionBuilder(Type actionType, WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
        {
            this.actionType = actionType;
            this.worldKeyBuilder = worldKeyBuilder;
            this.targetKeyBuilder = targetKeyBuilder;
            
            this.config = new ActionConfig
            {
                Name = actionType.Name,
                ClassType = actionType.AssemblyQualifiedName,
                BaseCost = 1,
                InRange = 0.5f,
                RequiresTarget = true,
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
        
        public ActionBuilder SetRequiresTarget(bool requiresTarget)
        {
            this.config.RequiresTarget = requiresTarget;
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

        [Obsolete("Use `AddEffect<TWorldKey>(EffectType type)` instead.")]
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

        public ActionBuilder AddEffect<TWorldKey>(EffectType type)
            where TWorldKey : IWorldKey
        {
            this.effects.Add(new Effect
            {
                WorldKey = this.worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = type == EffectType.Increase
            });
            
            return this;
        }

        public ActionBuilder SetProperties(IActionProperties properties)
        {
            this.ValidateProperties(properties);
            
            this.config.Properties = properties;
            return this;
        }

        private void ValidateProperties(IActionProperties properties)
        {
            var actionPropsType = this.GetPropertiesType();
            
            if (actionPropsType == properties.GetType())
                return;
                
            throw new ArgumentException($"The provided properties do not match the expected type '{actionPropsType.Name}'.", nameof(properties));
        }
        
        private Type GetPropertiesType()
        {
            var baseType = this.actionType.BaseType;
            
            if (baseType == null)
                return null;
            
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ActionBase<,>)) 
                return baseType.GetGenericArguments()[1];
            
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ActionBase<>)) 
                return typeof(EmptyActionProperties);
            
            return null;
        }

        public IActionConfig Build()
        {
            this.config.Conditions = this.conditions.ToArray();
            this.config.Effects = this.effects.ToArray();
            
            return this.config;
        }
        
        public static ActionBuilder Create<TAction>(WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
            where TAction : IAction
        {
            return new ActionBuilder(typeof(TAction), worldKeyBuilder, targetKeyBuilder);
        }
    }
}