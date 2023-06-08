using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolvers;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
{
    public class GoapSetFactory
    {
        private readonly IGoapConfig goapConfig;
        private readonly ClassResolver classResolver = new();
        private IGoapSetConfigValidatorRunner goapSetConfigValidatorRunner = new GoapSetConfigValidatorRunner();

        public GoapSetFactory(IGoapConfig goapConfig)
        {
            this.goapConfig = goapConfig;
        }
        
        public GoapSet Create(IGoapSetConfig config)
        {
            this.Validate(config);
            
            var sensorRunner = this.CreateSensorRunner(config);

            return new GoapSet(
                id: config.Name,
                config: this.goapConfig,
                actions: this.GetActions(config),
                goals: this.GetGoals(config),
                sensorRunner: sensorRunner,
                debugger: this.GetDebugger(config)
            );
        }
        
        private void Validate(IGoapSetConfig config)
        {
            var results = this.goapSetConfigValidatorRunner.Validate(config);

            foreach (var error in results.GetErrors())
            {
                Debug.LogError(error);
            }
            
            foreach (var warning in results.GetWarnings())
            {
                Debug.LogWarning(warning);
            }
            
            if (results.HasErrors())
                throw new GoapException($"GoapSetConfig has errors: {config.Name}");
        }
        
        private SensorRunner CreateSensorRunner(IGoapSetConfig config)
        {
            return new SensorRunner(this.GetWorldSensors(config), this.GetTargetSensors(config));
        }
        
        private List<IActionBase> GetActions(IGoapSetConfig config)
        {
            var actions = this.classResolver.Load<IActionBase, IActionConfig>(config.Actions);
            var injector = this.goapConfig.GoapInjector;
            
            actions.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });

            return actions;
        }
        
        private List<IGoalBase> GetGoals(IGoapSetConfig config)
        {
            var goals = this.classResolver.Load<IGoalBase, IGoalConfig>(config.Goals);
            var injector = this.goapConfig.GoapInjector;
            
            goals.ForEach(x =>
            {
                injector.Inject(x);
            });

            return goals;
        }
        
        private List<IWorldSensor> GetWorldSensors(IGoapSetConfig config)
        {
            var worldSensors = this.classResolver.Load<IWorldSensor, IWorldSensorConfig>(config.WorldSensors);
            var injector = this.goapConfig.GoapInjector;
            
            worldSensors.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });
            
            return worldSensors;
        }
        
        private List<ITargetSensor> GetTargetSensors(IGoapSetConfig config)
        {
            var targetSensors = this.classResolver.Load<ITargetSensor, ITargetSensorConfig>(config.TargetSensors);
            var injector = this.goapConfig.GoapInjector;
            
            targetSensors.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });
            
            return targetSensors;
        }

        private IAgentDebugger GetDebugger(IGoapSetConfig config)
        {
            return this.classResolver.Load<IAgentDebugger>(config.DebuggerClass);
        }
    }
}