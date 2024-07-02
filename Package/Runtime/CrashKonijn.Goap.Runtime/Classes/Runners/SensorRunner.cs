using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class SensorRunner : ISensorRunner
    {
        private SensorSet defaultSet = new();
        private Dictionary<IGoapAction, SensorSet> actionSets = new();
        private Dictionary<Type, ISensor> sensors = new();

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
                this.defaultSet.AddSensor(worldSensor);
                
                this.sensors.Add(worldSensor.Key.GetType(), worldSensor);
            }

            foreach (var targetSensor in targetSensors)
            {
                this.defaultSet.AddSensor(targetSensor);
                
                this.sensors.Add(targetSensor.Key.GetType(), targetSensor);
            }
            
            foreach (var multiSensor in multiSensors)
            {
                this.defaultSet.AddSensor(multiSensor);
                
                foreach (var key in multiSensor.GetKeys())
                {
                    this.sensors.Add(key, multiSensor);
                }
            }
        }

        public void Update()
        {
            foreach (var localSensor in this.defaultSet.LocalSensors)
            {
                localSensor.Update();
            }
        }

        public void Update(IGoapAction action)
        {
            var set = this.GetSet(action);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Update();
            }
        }

        public void SenseGlobal()
        {
            foreach (var globalSensor in this.defaultSet.GlobalSensors)
            {
                globalSensor.Sense(this.worldData);
            }
        }

        public void SenseGlobal(IGoapAction action)
        {
            var set = this.GetSet(action);
            
            foreach (var globalSensor in set.GlobalSensors)
            {
                globalSensor.Sense(this.worldData);
            }
        }

        public void SenseLocal(IMonoGoapActionProvider actionProvider)
        {
            foreach (var localSensor in this.defaultSet.LocalSensors)
            {
                localSensor.Sense(actionProvider.WorldData, actionProvider.Agent, actionProvider.Agent.Injector);
            }
        }
        
        public void SenseLocal(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            if (actionProvider.IsNull())
                return;
            
            if (action == null)
                return;
            
            var set = this.GetSet(action);
            
            foreach (var localSensor in set.LocalSensors)
            {
                localSensor.Sense(actionProvider.WorldData, actionProvider.Agent, actionProvider.Agent.Injector);
            }
        }

        private SensorSet GetSet(IGoapAction action)
        {
            if (this.actionSets.TryGetValue(action, out var existingSet))
                return existingSet;
            
            return this.CreateSet(action);
        }

        private SensorSet CreateSet(IGoapAction action)
        {
            var set = new SensorSet();
                
            foreach (var condition in action.Conditions)
            {
                set.Keys.Add(condition.WorldKey.GetType());
            }
                
            if (action.Config.Target != null)
                set.Keys.Add(action.Config.Target.GetType());
                
            foreach (var key in set.Keys)
            {
                if (this.sensors.TryGetValue(key, out var sensor))
                {
                    set.AddSensor(sensor);
                }
            }
                
            this.actionSets[action] = set;

            return set;
        }
    }

    public class SensorSet
    {
        public HashSet<Type> Keys { get; } = new();
        public HashSet<ILocalSensor> LocalSensors { get; } = new();
        public HashSet<IGlobalSensor> GlobalSensors { get; } = new();
        
        public void AddSensor(ISensor sensor)
        {
            switch (sensor)
            {
                case IMultiSensor multiSensor:
                    this.LocalSensors.Add(multiSensor);
                    this.GlobalSensors.Add(multiSensor);
                    break;
                case ILocalSensor localSensor:
                    this.LocalSensors.Add(localSensor);
                    break;
                case IGlobalSensor globalSensor:
                    this.GlobalSensors.Add(globalSensor);
                    break;
            }
        }
    }
}