using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class MultiSensorBase : IMultiSensor
    {
        public IMultiSensorConfig Config { get; private set; }

        public Dictionary<Type, LocalSensor> LocalSensors { get; private set; } = new();
        public Dictionary<Type, GlobalSensor> GlobalSensors { get; private set; } = new();

        public void SetConfig(IMultiSensorConfig config)
        {
            this.Config = config;
        }

        public abstract void Created();

        public abstract void Update();
        
        public Type[] GetKeys()
        {
            var keys = new List<Type>();
            
            foreach (var sensor in this.LocalSensors.Values)
            {
                keys.Add(sensor.Key);
            }
            
            foreach (var sensor in this.GlobalSensors.Values)
            {
                keys.Add(sensor.Key);
            }

            return keys.ToArray();
        }
        
        public void Sense(IWorldData data)
        {
            foreach (var sensor in this.GlobalSensors.Values)
            {
                sensor.Sense(data);
            }
        }

        public void Sense(IWorldData data, Type[] keys)
        {
            foreach (var key in keys)
            {
                if (this.GlobalSensors.ContainsKey(key))
                {
                    this.GlobalSensors[key].Sense(data);
                }
            }
        }

        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references)
        {
            foreach (var sensor in this.LocalSensors.Values)
            {
                sensor.Sense(data, agent, references);
            }
        }
        
        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references, Type[] keys)
        {
            foreach (var key in keys)
            {
                if (this.LocalSensors.ContainsKey(key))
                {
                    this.LocalSensors[key].Sense(data, agent, references);
                }
            }
        }

        public void AddLocalWorldSensor<TKey>(Func<IActionReceiver, IComponentReference, SenseValue> sense, ISensorTimer timer = null)
            where TKey : IWorldKey
        {
            timer ??= SensorTimer.Always;
            
            this.LocalSensors.Add(typeof(TKey), new LocalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data, IActionReceiver agent, IComponentReference references) =>
                {
                    var state = data.GetWorldState(typeof(TKey));
                    
                    if (!timer.ShouldSense(state?.Timer))
                        return;
                    
                    data.SetState<TKey>(sense(agent, references));
                }
            });
        }

        public void AddGlobalWorldSensor<TKey>(Func<SenseValue> sense, ISensorTimer timer = null)
            where TKey : IWorldKey
        {
            timer ??= SensorTimer.Always;
            
            this.GlobalSensors.Add(typeof(TKey), new GlobalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data) =>
                {
                    var state = data.GetWorldState(typeof(TKey));
                    
                    if (!timer.ShouldSense(state?.Timer))
                        return;
                    
                    data.SetState<TKey>(sense());
                }
            });
        }

        public void AddLocalTargetSensor<TKey>(Func<IActionReceiver, IComponentReference, ITarget, ITarget> sense, ISensorTimer timer = null)
            where TKey : ITargetKey
        {
            timer ??= SensorTimer.Always;
            
            this.LocalSensors.Add(typeof(TKey), new LocalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data, IActionReceiver agent, IComponentReference references) =>
                {
                    var state = data.GetTargetState(typeof(TKey));
                    
                    if (!timer.ShouldSense(state?.Timer))
                        return;
                    
                    data.SetTarget<TKey>(sense(agent, references, state?.Value));
                }
            });
        }

        public void AddGlobalTargetSensor<TKey>(Func<ITarget, ITarget> sense, ISensorTimer timer = null)
            where TKey : ITargetKey
        {
            timer ??= SensorTimer.Always;
            
            this.GlobalSensors.Add(typeof(TKey), new GlobalSensor
            {
                Key = typeof(TKey),
                Sense = (IWorldData data) =>
                {
                    var state = data.GetTargetState(typeof(TKey));
                    
                    if (!timer.ShouldSense(state?.Timer))
                        return;
                    
                    data.SetTarget<TKey>(sense(state?.Value));
                }
            });
        }
        
        public string[] GetSensors()
        {
            var sensors = new List<string>();
            
            foreach (var sensor in this.LocalSensors.Values)
            {
                sensors.Add($"{sensor.Key.Name} (local)");
            }
            
            foreach (var sensor in this.GlobalSensors.Values)
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
        public Action<IWorldData, IActionReceiver, IComponentReference> Sense;
    }
}