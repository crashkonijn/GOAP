using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolvers;

namespace CrashKonijn.Goap.Classes
{
    public class GoapSetFactory
    {
        private readonly GoapConfig goapConfig;

        public GoapSetFactory(GoapConfig goapConfig)
        {
            this.goapConfig = goapConfig;
        }
        
        public GoapSet Create(IGoapSetConfig config)
        {
            var sensorRunner = this.CreateSensorRunner(config);

            return new GoapSet(
                id: config.Name,
                config: this.goapConfig,
                actions: this.GetActions(config),
                goals: this.GetGoals(config),
                sensorRunner: sensorRunner
            );
        }
        
        private SensorRunner CreateSensorRunner(IGoapSetConfig config)
        {
            var worldSensors = new ClassResolver().Load<IWorldSensor, IWorldSensorConfig>(config.WorldSensors);
            var targetSensors = new ClassResolver().Load<ITargetSensor, ITargetSensorConfig>(config.TargetSensors);

            return new SensorRunner(worldSensors, targetSensors);
        }
        
        private List<IActionBase> GetActions(IGoapSetConfig config)
        {
            var actions = new ClassResolver().Load<IActionBase, IActionConfig>(config.Actions);
            var actionInjector = this.goapConfig.GoapInjector;
            
            actions.ForEach(x => actionInjector.Inject(x));

            return actions;
        }
        
        private List<IGoalBase> GetGoals(IGoapSetConfig config)
        {
            var goals = new ClassResolver().Load<IGoalBase, IGoalConfig>(config.Goals);
            var goalInjector = this.goapConfig.GoapInjector;
            
            goals.ForEach(x => goalInjector.Inject(x));

            return goals;
        }
    }
}