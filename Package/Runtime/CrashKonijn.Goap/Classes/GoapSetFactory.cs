using System.Linq;
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
                config: this.goapConfig,
                actions: config.Actions.Cast<IActionBase>().ToHashSet(), // Todo, load through class resolver
                goals: new ClassResolver().Load<IGoalBase, IGoalConfig>(config.Goals),
                sensorRunner: sensorRunner
            );
        }
        
        private SensorRunner CreateSensorRunner(IGoapSetConfig config)
        {
            var worldSensors = new ClassResolver().Load<IWorldSensor, IWorldSensorConfig>(config.WorldSensors);
            var targetSensors = new ClassResolver().Load<ITargetSensor, ITargetSensorConfig>(config.TargetSensors);

            return new SensorRunner(worldSensors, targetSensors);
        }
    }
}