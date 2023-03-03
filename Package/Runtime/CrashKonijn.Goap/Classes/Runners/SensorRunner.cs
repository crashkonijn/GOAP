using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class SensorRunner
    {
        private HashSet<ILocalWorldSensor> localWorldSensors = new();
        private HashSet<IGlobalWorldSensor> globalWorldSensors = new();
        private HashSet<ILocalTargetSensor> localTargetSensors = new();
        private HashSet<IGlobalTargetSensor> globalTargetSensors = new();
        
        // GC caches
        private LocalWorldData localWorldData;
        private readonly GlobalWorldData worldData = new();

        public SensorRunner(IEnumerable<IWorldSensor> worldSensors, IEnumerable<ITargetSensor> targetSensors)
        {
            foreach (var worldSensor in worldSensors)
            {
                switch (worldSensor)
                {
                    case ILocalWorldSensor localSensor:
                        this.localWorldSensors.Add(localSensor);
                        break;
                    case IGlobalWorldSensor globalSensor:
                        this.globalWorldSensors.Add(globalSensor);
                        break;
                }
            }

            foreach (var targetSensor in targetSensors)
            {
                switch (targetSensor)
                {
                    case ILocalTargetSensor localSensor:
                        this.localTargetSensors.Add(localSensor);
                        break;
                    case IGlobalTargetSensor globalSensor:
                        this.globalTargetSensors.Add(globalSensor);
                        break;
                }
            }
        }

        public void Update()
        {
            foreach (var localWorldSensor in this.localWorldSensors)
            {
                localWorldSensor.Update();
            }

            foreach (var localTargetSensor in this.localTargetSensors)
            {
                localTargetSensor.Update();
            }
        }

        public GlobalWorldData SenseGlobal()
        {
            foreach (var globalWorldSensor in this.globalWorldSensors)
            {
                this.worldData.SetState(globalWorldSensor.Key, globalWorldSensor.Sense() ? WorldKeyState.True : WorldKeyState.False);
            }
            
            foreach (var globalTargetSensor in this.globalTargetSensors)
            {
                this.worldData.SetTarget(globalTargetSensor.Key, globalTargetSensor.Sense());
            }

            return this.worldData;
        }

        public LocalWorldData SenseLocal(GlobalWorldData worldData, IMonoAgent agent)
        {
            this.localWorldData = (LocalWorldData) agent.WorldData;

            this.localWorldData.Apply(worldData);
            
            foreach (var localWorldSensor in this.localWorldSensors)
            {
                this.localWorldData.SetState(localWorldSensor.Key, localWorldSensor.Sense(agent) ? WorldKeyState.True : WorldKeyState.False);
            }
            
            foreach (var localTargetSensor in this.localTargetSensors)
            {
                this.localWorldData.SetTarget(localTargetSensor.Key, localTargetSensor.Sense(agent));
            }
            
            return this.localWorldData;
        }
    }
}