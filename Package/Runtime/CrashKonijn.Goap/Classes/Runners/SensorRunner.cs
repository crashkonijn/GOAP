using System.Collections.Generic;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class SensorRunner : ISensorRunner
    {
        private HashSet<ILocalSensor> localSensors = new();
        private HashSet<IGlobalSensor> globalSensors = new();

        private IGlobalWorldData worldData;

        public SensorRunner(
            IEnumerable<IWorldSensor> worldSensors,
            IEnumerable<ITargetSensor> targetSensors,
            IEnumerable<IMultiSensor> multiSensors,
            IGlobalWorldData globalWorldData
        ) {
            this.worldData = globalWorldData;
            
            foreach (var worldSensor in worldSensors)
            {
                switch (worldSensor)
                {
                    case ILocalWorldSensor localSensor:
                        this.localSensors.Add(localSensor);
                        break;
                    case IGlobalWorldSensor globalSensor:
                        this.globalSensors.Add(globalSensor);
                        break;
                }
            }

            foreach (var targetSensor in targetSensors)
            {
                switch (targetSensor)
                {
                    case ILocalTargetSensor localSensor:
                        this.localSensors.Add(localSensor);
                        break;
                    case IGlobalTargetSensor globalSensor:
                        this.globalSensors.Add(globalSensor);
                        break;
                }
            }
            
            foreach (var multiSensor in multiSensors)
            {
                this.localSensors.Add(multiSensor);
                this.globalSensors.Add(multiSensor);
            }
        }

        public void Update()
        {
            foreach (var localSensor in this.localSensors)
            {
                localSensor.Update();
            }
        }

        public void SenseGlobal()
        {
            foreach (var globalSensor in this.globalSensors)
            {
                globalSensor.Sense(this.worldData);
            }
        }

        public void SenseLocal(IMonoAgent agent)
        {
            foreach (var localSensor in this.localSensors)
            {
                localSensor.Sense(agent.WorldData, agent, agent.Injector);
            }
        }
    }
}