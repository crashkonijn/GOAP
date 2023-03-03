using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
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
        private GlobalWorldData worldData;

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

        public GlobalWorldData SenseGlobal()
        {
            this.worldData = new GlobalWorldData();

            this.worldData.States = this.globalWorldSensors
                .Where(x => x.Sense())
                .Select(x => x.Key)
                .ToHashSet();

            this.worldData.Targets = this.globalTargetSensors
                .ToDictionary(x => x.Key, y => y.Sense());

            return this.worldData;
        }

        public LocalWorldData SenseLocal(GlobalWorldData worldData, IMonoAgent agent)
        {
            this.localWorldData = new LocalWorldData(worldData);

            this.localWorldData.AddStates(this.localWorldSensors
                .Where(x => x.Sense(agent))
                .Select(x => x.Key));

            this.localWorldData.AddTargets(this.localTargetSensors
                .ToDictionary(x => x.Key, y => y.Sense(agent)));

            return this.localWorldData;
        }
    }
}