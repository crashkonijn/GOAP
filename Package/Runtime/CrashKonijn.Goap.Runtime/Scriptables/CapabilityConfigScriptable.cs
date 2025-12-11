using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [CreateAssetMenu(menuName = "Goap/CapabilityConfig")]
    public class CapabilityConfigScriptable : ScriptableCapabilityFactoryBase
    {
        public List<CapabilityGoal> goals = new();
        public List<CapabilityAction> actions = new();
        public List<CapabilityWorldSensor> worldSensors = new();
        public List<CapabilityTargetSensor> targetSensors = new();
        public List<CapabilityMultiSensor> multiSensors = new();

        [SerializeField]
        public GeneratorScriptable generatorScriptable;

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
            var worldKeys = generator.GetWorldKeys();
            
            var configs = new List<IActionConfig>();
            
            foreach (var x in this.actions)
            {
                var conditions = new ICondition[x.conditions.Count];

                for (var i = 0; i < x.conditions.Count; i++)
                {
                    var condition = x.conditions[i];
                    conditions[i] = new ValueCondition
                    {
                        WorldKey = condition.worldKey.GetScript(worldKeys).GetInstance<IWorldKey>(),
                        Comparison = condition.comparison,
                        Amount = condition.amount,
                    };
                }
                
                var effects = new IEffect[x.effects.Count];
                
                for (var i = 0; i < x.effects.Count; i++)
                {
                    var effect = x.effects[i];
                    effects[i] = new Effect
                    {
                        WorldKey = effect.worldKey.GetScript(worldKeys).GetInstance<IWorldKey>(),
                        Increase = effect.effect == EffectType.Increase,
                    };
                }
                
                configs.Add(new ActionConfig
                {
                    Name = x.action.Name,
                    ClassType = x.action.GetScript(actionClasses).GetFullName(),
                    BaseCost = x.baseCost,
                    Target = x.target.GetScript(targetClasses).GetInstance<ITargetKey>(),
                    StoppingDistance = x.stoppingDistance,
                    ValidateTarget = x.validateTarget,
                    RequiresTarget = x.requiresTarget,
                    ValidateConditions = x.validateConditions,
                    Conditions = conditions,
                    Effects = effects,
                    MoveMode = x.moveMode,
                    Properties = x.properties,
                });
            }

            return configs;
        }

        public List<IGoalConfig> GetGoals(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IGoalConfig>();

            var goalClasses = generator.GetGoals();
            var worldKeys = generator.GetWorldKeys();
            
            var configs = new List<IGoalConfig>();
            
            foreach (var goal in this.goals)
            {
                var conditions = new List<ICondition>();
                
                foreach (var condition in goal.conditions)
                {
                    conditions.Add(new ValueCondition
                    {
                        WorldKey = condition.worldKey.GetScript(worldKeys).GetInstance<IWorldKey>(),
                        Comparison = condition.comparison,
                        Amount = condition.amount,
                    });
                }
                
                configs.Add(new GoalConfig
                {
                    Name = goal.goal.Name,
                    ClassType = goal.goal.GetScript(goalClasses).GetFullName(),
                    Conditions = conditions,
                });
            }

            return configs;
        }

        public List<IWorldSensorConfig> GetWorldSensors(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IWorldSensorConfig>();

            var sensorClasses = generator.GetWorldSensors();
            var worldKeys = generator.GetWorldKeys();
            
            var configs = new List<IWorldSensorConfig>(this.worldSensors.Count);
            
            foreach (var worldSensor in this.worldSensors)
            {
                configs.Add(new WorldSensorConfig
                {
                    Name = worldSensor.sensor.Name,
                    ClassType = worldSensor.sensor.GetScript(sensorClasses).GetFullName(),
                    Key = worldSensor.worldKey.GetScript(worldKeys).GetInstance<IWorldKey>(),
                });
            }
            
            return configs;
        }

        public List<ITargetSensorConfig> GetTargetSensors(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<ITargetSensorConfig>();

            var sensorClasses = generator.GetTargetSensors();
            var targetKeys = generator.GetTargetKeys();
            
            var configs = new List<ITargetSensorConfig>(this.targetSensors.Count);

            foreach (var targetSensor in this.targetSensors)
            {
                configs.Add(new TargetSensorConfig
                {
                    Name = targetSensor.sensor.Name,
                    ClassType = targetSensor.sensor.GetScript(sensorClasses).GetFullName(),
                    Key = targetSensor.targetKey.GetScript(targetKeys).GetInstance<ITargetKey>(),
                });
            }

            return configs;
        }

        public List<IMultiSensorConfig> GetMultiSensors(GeneratorScriptable generator)
        {
            if (generator == null)
                return new List<IMultiSensorConfig>();

            var sensorClasses = generator.GetMultiSensors();
            
            var configs = new List<IMultiSensorConfig>(this.multiSensors.Count);

            foreach (var multiSensor in this.multiSensors)
            {
                configs.Add(new MultiSensorConfig
                {
                    Name = multiSensor.sensor.Name,
                    ClassType = multiSensor.sensor.GetScript(sensorClasses).GetFullName(),
                });
            }

            return configs;
        }

        public GeneratorScriptable GetGenerator()
        {
#if UNITY_EDITOR
            this.generatorScriptable = ClassScanner.GetGenerator(this);
            EditorUtility.SetDirty(this);
#endif
            return this.generatorScriptable;
        }

        public void FixIssues()
        {
#if UNITY_EDITOR
            var validator = new ScriptReferenceValidator();
                
            var issues = validator.CheckAll(this);
                
            if (issues.Length == 0)
            {
                return;
            }
           
            foreach (var issue in issues)
            {
                issue.Fix(this.GetGenerator());
            }
                
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
#endif
        }

        private void OnValidate()
        {
            this.FixIssues();
        }
    }
}
