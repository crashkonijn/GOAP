using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Resolvers;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
{
    public class AgentTypeFactory
    {
        private readonly IGoapConfig goapConfig;
        private readonly ClassResolver classResolver = new();
        private IAgentTypeConfigValidatorRunner agentTypeConfigValidatorRunner = new AgentTypeConfigValidatorRunner();

        public AgentTypeFactory(IGoapConfig goapConfig)
        {
            this.goapConfig = goapConfig;
        }
        
        public AgentType Create(IAgentTypeConfig config)
        {
            this.Validate(config);
            
            var sensorRunner = this.CreateSensorRunner(config);

            return new AgentType(
                id: config.Name,
                config: this.goapConfig,
                actions: this.GetActions(config),
                goals: this.GetGoals(config),
                sensorRunner: sensorRunner,
                debugger: this.GetDebugger(config)
            );
        }
        
        private void Validate(IAgentTypeConfig config)
        {
            var results = this.agentTypeConfigValidatorRunner.Validate(config);

            foreach (var error in results.GetErrors())
            {
                Debug.LogError(error);
            }
            
            foreach (var warning in results.GetWarnings())
            {
                Debug.LogWarning(warning);
            }
            
            if (results.HasErrors())
                throw new GoapException($"AgentTypeConfig has errors: {config.Name}");
        }
        
        private SensorRunner CreateSensorRunner(IAgentTypeConfig config)
        {
            return new SensorRunner(this.GetWorldSensors(config), this.GetTargetSensors(config));
        }
        
        private List<IAction> GetActions(IAgentTypeConfig config)
        {
            var actions = this.classResolver.Load<IAction, IActionConfig>(config.Actions);
            var injector = this.goapConfig.GoapInjector;
            
            actions.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });

            return actions;
        }
        
        private List<IGoal> GetGoals(IAgentTypeConfig config)
        {
            var goals = this.classResolver.Load<IGoal, IGoalConfig>(config.Goals);
            var injector = this.goapConfig.GoapInjector;
            
            goals.ForEach(x =>
            {
                injector.Inject(x);
            });

            return goals;
        }
        
        private List<IWorldSensor> GetWorldSensors(IAgentTypeConfig config)
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
        
        private List<ITargetSensor> GetTargetSensors(IAgentTypeConfig config)
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

        private IAgentDebugger GetDebugger(IAgentTypeConfig config)
        {
            return this.classResolver.Load<IAgentDebugger>(config.DebuggerClass);
        }
    }
}