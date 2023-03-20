using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using UnityEngine;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class GoapSetBuilder
    {
        private readonly GoapSetConfig set;

        private readonly List<ActionBuilder> actionBuilders = new();
        private readonly List<GoalBuilder> goalBuilders = new();
        private readonly List<TargetSensorBuilder> targetSensorBuilders = new();
        private readonly List<WorldSensorBuilder> worldSensorBuilders = new();
        private readonly WorldKeyBuilder worldKeyBuilder = new();
        private readonly TargetKeyBuilder targetKeyBuilder = new();

        public GoapSetBuilder(string name)
        {
            this.set = new GoapSetConfig(name);
        }
        
        public ActionBuilder AddAction<TAction>()
            where TAction : IActionBase
        {
            var actionBuilder = ActionBuilder.Create<TAction>(this.worldKeyBuilder, this.targetKeyBuilder);
            
            this.actionBuilders.Add(actionBuilder);
            
            return actionBuilder;
        }
        
        public GoalBuilder AddGoal<TGoal>()
            where TGoal : IGoalBase
        {
            var goalBuilder = GoalBuilder.Create<TGoal>(this.worldKeyBuilder);

            this.goalBuilders.Add(goalBuilder);
            
            return goalBuilder;
        }
        
        public WorldSensorBuilder AddWorldSensor<TWorldSensor>()
            where TWorldSensor : IWorldSensor
        {
            var worldSensorBuilder = WorldSensorBuilder.Create<TWorldSensor>(this.worldKeyBuilder);

            this.worldSensorBuilders.Add(worldSensorBuilder);
            
            return worldSensorBuilder;
        }
        
        public TargetSensorBuilder AddTargetSensor<TTargetSensor>()
            where TTargetSensor : ITargetSensor
        {
            var targetSensorBuilder = TargetSensorBuilder.Create<TTargetSensor>(this.targetKeyBuilder);

            this.targetSensorBuilders.Add(targetSensorBuilder);
            
            return targetSensorBuilder;
        }

        public WorldKeyBuilder GetWorldKeyBuilder()
        {
            return this.worldKeyBuilder;
        }
        
        public GoapSetConfig Build()
        {
            this.set.Actions = this.actionBuilders.Select(x => x.Build()).ToList();
            this.set.Goals = this.goalBuilders.Select(x => x.Build()).ToList();
            this.set.TargetSensors = this.targetSensorBuilders.Select(x => x.Build()).ToList();
            this.set.WorldSensors = this.worldSensorBuilders.Select(x => x.Build()).ToList();
            
            return this.set;
        }
    }
    
    public class TargetSensorBuilder
    {
        private readonly TargetKeyBuilder targetKeyBuilder;
        private readonly TargetSensorConfig config;

        public TargetSensorBuilder(Type type, TargetKeyBuilder targetKeyBuilder)
        {
            this.targetKeyBuilder = targetKeyBuilder;
            this.config = new TargetSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public TargetSensorBuilder SetTarget(string key)
        {
            this.config.Key = this.targetKeyBuilder.GetKey(key);
            
            return this;
        }
        
        public TargetSensorBuilder SetTarget<T1>(string key)
        {
            this.config.Key = this.targetKeyBuilder.GetKey<T1>(key);
            
            return this;
        }
        
        public TargetSensorBuilder SetTarget<T1, T2>(string key)
        {
            this.config.Key = this.targetKeyBuilder.GetKey<T1, T2>(key);
            
            return this;
        }
        
        public ITargetSensorConfig Build()
        {
            return this.config;
        }

        public static TargetSensorBuilder Create<TTargetSensor>(TargetKeyBuilder targetKeyBuilder) where TTargetSensor : ITargetSensor
        {
            return new TargetSensorBuilder(typeof(TTargetSensor), targetKeyBuilder);
        }
    }
    
    public class WorldSensorBuilder
    {
        private readonly WorldKeyBuilder worldKeyBuilder;
        private readonly WorldSensorConfig config;

        public WorldSensorBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            this.config = new WorldSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public WorldSensorBuilder SetKey(string key)
        {
            this.config.Key = this.worldKeyBuilder.GetKey(key);
            
            return this;
        }
        
        public WorldSensorBuilder SetKey<T1>(string key)
        {
            this.config.Key = this.worldKeyBuilder.GetKey<T1>(key);
            
            return this;
        }
        
        public WorldSensorBuilder SetKey<T1, T2>(string key)
        {
            this.config.Key = this.worldKeyBuilder.GetKey<T1, T2>(key);
            
            return this;
        }
        
        public IWorldSensorConfig Build()
        {
            return this.config;
        }

        public static WorldSensorBuilder Create<TWorldSensor>(WorldKeyBuilder worldKeyBuilder)
            where TWorldSensor : IWorldSensor
        {
            return new WorldSensorBuilder(typeof(TWorldSensor), worldKeyBuilder);
        }
    }

    public class WorldKeyBuilder : KeyBuilderBase<WorldKey, IWorldKey>
    {

    }

    public class TargetKeyBuilder : KeyBuilderBase<TargetKey, ITargetKey>
    {
    }
    
    public abstract class KeyBuilderBase<TKey, TInterface>
        where TKey : TInterface
    {
        private Dictionary<string, TKey> keys = new();
        
        public TInterface GetKey(string name)
        {
            if (this.keys.TryGetValue(name, out var key))
            {
                return key;
            }

            key = (TKey) Activator.CreateInstance(typeof(TKey), name);
            this.keys.Add(name, key);
            
            return key;
        }
        
        public TInterface GetKey<T>(string name)
        {
            var dynamicName = $"{name}<{typeof(T).Name}>";
            
            if (this.keys.TryGetValue(dynamicName, out var key))
            {
                return key;
            }

            key = (TKey) Activator.CreateInstance(typeof(TKey), dynamicName);
            this.keys.Add(dynamicName, key);
            
            return key;
        }
        
        public TInterface GetKey<T1, T2>(string name)
        {
            var dynamicName = $"{name}<{typeof(T1).Name},{typeof(T2).Name}>";
            
            if (this.keys.TryGetValue(dynamicName, out var key))
            {
                return key;
            }

            key = (TKey) Activator.CreateInstance(typeof(TKey), dynamicName);
            this.keys.Add(dynamicName, key);
            
            return key;
        }
    }

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
        
        public GoalBuilder AddCondition(string key, Comparison comparison, int amount)
        {
            this.conditions.Add(new Condition(this.worldKeyBuilder.GetKey(key), comparison, amount));
            return this;
        }
        
        public GoalBuilder AddCondition<T>(string key, Comparison comparison, int amount)
        {
            this.conditions.Add(new Condition(this.worldKeyBuilder.GetKey<T>(key), comparison, amount));
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