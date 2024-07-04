using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Runtime
{
    [CreateAssetMenu(menuName = "Goap/CapabilityConfig")]
    public class CapabilityConfigScriptable : ScriptableCapabilityFactoryBase
    {
        public List<BehaviourGoal> goals = new();
        public List<BehaviourAction> actions = new();
        public List<BehaviourWorldSensor> worldSensors = new();
        public List<BehaviourTargetSensor> targetSensors = new();
        public List<BehaviourMultiSensor> multiSensors = new();

        public override ICapabilityConfig Create()
        {
            var generator = this.GetGenerator();

            return new CapabilityConfig(this.name)
            {
                Goals = this.GetGoals(generator),
                Actions = this.GetActions(generator),
                WorldSensors = this.GetWorldSensors(generator),
                TargetSensors = this.GetTargetSensors(generator),
                MultiSensors = this.GetMultiSensors(generator),
            };
        }

        public List<IActionConfig> GetActions(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IActionConfig>();
            
            var actionClasses = generator.GetActions();
            var targetClasses = generator.GetTargetKeys();
            
            return this.actions.Select(x => new ActionConfig
            {
                Name = x.action.Name,
                ClassType = x.action.GetScript(actionClasses).GetFullName(),
                BaseCost = x.baseCost,
                Target = x.target.GetScript(targetClasses).GetInstance<ITargetKey>(),
                StoppingDistance = x.stoppingDistance,
                RequiresTarget = x.requiresTarget,
                ValidateConditions = x.validateConditions,
                Conditions = x.conditions.Select(y => new Condition
                {
                    WorldKey = y.worldKey.GetScript(generator.GetWorldKeys()).GetInstance<IWorldKey>(),
                    Comparison = y.comparison,
                    Amount = y.amount
                }).Cast<ICondition>().ToArray(),
                Effects = x.effects.Select(y => new Effect
                {
                    WorldKey = y.worldKey.GetScript(generator.GetWorldKeys()).GetInstance<IWorldKey>(),
                    Increase = y.effect == EffectType.Increase,
                }).Cast<IEffect>().ToArray(),
                MoveMode = x.moveMode,
                Properties = x.properties
            }).Cast<IActionConfig>().ToList();
        }

        public List<IGoalConfig> GetGoals(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IGoalConfig>();
            
            var goalClasses = generator.GetGoals();
            
            return this.goals.Select(x => new GoalConfig
            {
                Name = x.goal.Name,
                ClassType = x.goal.GetScript(goalClasses).GetFullName(),
                Conditions = x.conditions.Select(y => new Condition
                {
                    WorldKey = y.worldKey.GetScript(generator.GetWorldKeys()).GetInstance<IWorldKey>(),
                    Comparison = y.comparison,
                    Amount = y.amount
                }).Cast<ICondition>().ToList()
            }).Cast<IGoalConfig>().ToList();
        }

        public List<IWorldSensorConfig> GetWorldSensors(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IWorldSensorConfig>();
            
            var sensorClasses = generator.GetWorldSensors();
            
            return this.worldSensors.Select(x => new WorldSensorConfig
            {
                Name = x.sensor.Name,
                ClassType = x.sensor.GetScript(sensorClasses).GetFullName(),
                Key = x.worldKey.GetScript(generator.GetWorldKeys()).GetInstance<IWorldKey>()
            }).Cast<IWorldSensorConfig>().ToList();
        }
        
        public List<ITargetSensorConfig> GetTargetSensors(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<ITargetSensorConfig>();
            
            var sensorClasses = generator.GetTargetSensors();
            
            return this.targetSensors.Select(x => new TargetSensorConfig
            {
                Name = x.sensor.Name,
                ClassType = x.sensor.GetScript(sensorClasses).GetFullName(),
                Key = x.targetKey.GetScript(generator.GetTargetKeys()).GetInstance<ITargetKey>()
            }).Cast<ITargetSensorConfig>().ToList();
        }
        
        public List<IMultiSensorConfig> GetMultiSensors(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IMultiSensorConfig>();
            
            var sensorClasses = generator.GetMultiSensors();
            
            return this.multiSensors.Select(x => new MultiSensorConfig
            {
                Name = x.sensor.Name,
                ClassType = x.sensor.GetScript(sensorClasses).GetFullName()
            }).Cast<IMultiSensorConfig>().ToList();
        }
    }

    [Serializable]
    public class BehaviourGoal
    {
        public ClassRef goal = new();

        public float baseCost = 1;
        public List<BehaviourCondition> conditions = new();
    }
    
    [Serializable]
    public class BehaviourAction
    {
        public ClassRef action = new();
        public ClassRef target = new();
        
        [SerializeReference]
        public IActionProperties properties;
        
        public float baseCost = 1;
        public float stoppingDistance = 0.1f;
        public bool requiresTarget = true;
        public bool validateConditions = true;
        public ActionMoveMode moveMode;
        public List<BehaviourCondition> conditions = new();
        public List<BehaviourEffect> effects = new();
    }
    
    [Serializable]
    public class BehaviourCondition
    {
        // public string name => this.ToString();
        
        public ClassRef worldKey = new();
        public Comparison comparison;
        public int amount;

        public BehaviourCondition()
        {
            
        }
        
        public BehaviourCondition(string data)
        {
            var split = data.Split(' ');
            this.worldKey.Name = split[0];
            this.comparison = split[1].FromName();
            this.amount = int.Parse(split[2]);
        }

        public override string ToString() => $"{this.worldKey.Name} {this.comparison.ToName()} {this.amount}";
    }
    
    [Serializable]
    public class BehaviourEffect
    {
        // public string name => this.ToString();
        
        public ClassRef worldKey = new();
        public EffectType effect;

        public override string ToString() => $"{this.worldKey.Name}{this.effect.ToName()}";
    }

    [Serializable]
    public class BehaviourWorldSensor : BehaviourSensor
    {
        public ClassRef worldKey = new();

        public override string ToString() => $"{this.sensor.Name} ({this.worldKey.Name})";
    }

    [Serializable]
    public class BehaviourTargetSensor : BehaviourSensor
    {
        public ClassRef targetKey = new();

        public override string ToString() => $"{this.sensor.Name} ({this.targetKey.Name})";
    }

    [Serializable]
    public class BehaviourMultiSensor : BehaviourSensor
    {
        public override string ToString() => this.sensor.Name;
    }

    [Serializable]
    public abstract class BehaviourSensor
    {
        public ClassRef sensor = new();
    }

    [Serializable]
    public class ClassRef : IClassRef
    {
        [field: SerializeField]
        public string Name { get; set; }
        [field: SerializeField]
        public string Id  { get; set; }
    }
}