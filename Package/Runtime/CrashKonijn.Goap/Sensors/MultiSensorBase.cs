using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class MultiSensorBase : IMultiSensor
    {
        public IMultiSensorConfig Config { get; private set; }

        public List<LocalSensor> LocalSensors { get; private set; } = new();
        public List<GlobalSensor> GlobalSensors { get; private set; } = new();

        public void SetConfig(IMultiSensorConfig config)
        {
            this.Config = config;
        }

        public abstract void Created();

        public abstract void Update();
        
        public void Sense(IWorldData data)
        {
            this.GlobalSensors.ForEach(s => s.Sense(data));
        }

        public void Sense(IWorldData data, IMonoAgent agent, IComponentReference references)
        {
            this.LocalSensors.ForEach(s => s.Sense(data, agent, references));
        }

        public void AddLocalWorldSensor<TKey>(Func<IMonoAgent, IComponentReference, SenseValue> sense)
            where TKey : IWorldKey
        {
            this.LocalSensors.Add(new LocalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data, IMonoAgent agent, IComponentReference references) =>
                {
                    data.SetState<TKey>(sense(agent, references));
                }
            });
        }

        public void AddGlobalWorldSensor<TKey>(Func<SenseValue> sense)
            where TKey : IWorldKey
        {
            this.GlobalSensors.Add(new GlobalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data) =>
                {
                    data.SetState<TKey>(sense());
                }
            });
        }

        public void AddLocalTargetSensor<TKey>(Func<IMonoAgent, IComponentReference, ITarget> sense)
            where TKey : ITargetKey
        {
            this.LocalSensors.Add(new LocalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data, IMonoAgent agent, IComponentReference references) =>
                {
                    data.SetTarget<TKey>(sense(agent, references));
                }
            });
        }

        public void AddGlobalTargetSensor<TKey>(Func<ITarget> sense)
            where TKey : ITargetKey
        {
            this.GlobalSensors.Add(new GlobalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data) =>
                {
                    data.SetTarget<TKey>(sense());
                }
            });
        }
        
        public string[] GetSensors()
        {
            var sensors = new List<string>();
            
            foreach (var sensor in this.LocalSensors)
            {
                sensors.Add($"{sensor.Key.Name} (local)");
            }
            
            foreach (var sensor in this.GlobalSensors)
            {
                sensors.Add($"{sensor.Key.Name} (global)");
            }

            return sensors.ToArray();
        }
    }

    public class GlobalSensor
    {
        public Type Key;
        public Action<IWorldData> Sense;
    }

    public class LocalSensor
    {
        public Type Key;
        public Action<IWorldData, IMonoAgent, IComponentReference> Sense;
    }
}