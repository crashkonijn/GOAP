using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class MultiSensorBase : IMultiSensor
    {
        public IMultiSensorConfig Config { get; private set; }

        public Dictionary<Type, ILocalSensor> LocalSensors { get; private set; } = new();
        public Dictionary<Type, IGlobalSensor> GlobalSensors { get; private set; } = new();

        public void SetConfig(IMultiSensorConfig config)
        {
            this.Config = config;
        }

        public Type Key { get; set; }
        public abstract void Created();

        public abstract void Update();

        public Type[] GetKeys()
        {
            var keys = new List<Type>();

            foreach (var sensor in this.LocalSensors.Keys)
            {
                keys.Add(sensor);
            }

            foreach (var sensor in this.GlobalSensors.Keys)
            {
                keys.Add(sensor);
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
            this.ValidateCalledFromConstructor();

            timer ??= SensorTimer.Always;

            this.LocalSensors.Add(typeof(TKey), new LocalSensor
            {
                Key = typeof(TKey),
                SenseMethod = (IWorldData data, IActionReceiver agent, IComponentReference references) =>
                {
                    var state = data.GetWorldState(typeof(TKey));

                    if (!timer.ShouldSense(state?.Timer))
                        return;

                    data.SetState<TKey>(sense(agent, references));
                },
            });
        }

        public void AddGlobalWorldSensor<TKey>(Func<SenseValue> sense, ISensorTimer timer = null)
            where TKey : IWorldKey
        {
            this.ValidateCalledFromConstructor();

            timer ??= SensorTimer.Always;

            this.GlobalSensors.Add(typeof(TKey), new GlobalSensor
            {
                Key = typeof(TKey),
                SenseMethod = (IWorldData data) =>
                {
                    var state = data.GetWorldState(typeof(TKey));

                    if (!timer.ShouldSense(state?.Timer))
                        return;

                    data.SetState<TKey>(sense());
                },
            });
        }

        public void AddLocalTargetSensor<TKey>(Func<IActionReceiver, IComponentReference, ITarget, ITarget> sense, ISensorTimer timer = null)
            where TKey : ITargetKey
        {
            this.ValidateCalledFromConstructor();

            timer ??= SensorTimer.Always;

            this.LocalSensors.Add(typeof(TKey), new LocalSensor
            {
                Key = typeof(TKey),
                SenseMethod = (IWorldData data, IActionReceiver agent, IComponentReference references) =>
                {
                    var state = data.GetTargetState(typeof(TKey));

                    if (!timer.ShouldSense(state?.Timer))
                        return;

                    data.SetTarget<TKey>(sense(agent, references, state?.Value));
                },
            });
        }

        public void AddGlobalTargetSensor<TKey>(Func<ITarget, ITarget> sense, ISensorTimer timer = null)
            where TKey : ITargetKey
        {
            this.ValidateCalledFromConstructor();

            timer ??= SensorTimer.Always;

            this.GlobalSensors.Add(typeof(TKey), new GlobalSensor
            {
                Key = typeof(TKey),
                SenseMethod = (IWorldData data) =>
                {
                    var state = data.GetTargetState(typeof(TKey));

                    if (!timer.ShouldSense(state?.Timer))
                        return;

                    data.SetTarget<TKey>(sense(state?.Value));
                },
            });
        }

        public string[] GetSensors()
        {
            var sensors = new List<string>();

            foreach (var sensor in this.LocalSensors.Keys)
            {
                sensors.Add($"{sensor.Name} (local)");
            }

            foreach (var sensor in this.GlobalSensors.Keys)
            {
                sensors.Add($"{sensor.Name} (global)");
            }

            return sensors.ToArray();
        }

        private void ValidateCalledFromConstructor()
        {
#if UNITY_EDITOR
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();

            // Check if any of the frames belong to the constructor of this class
            var calledFromConstructor = frames != null && frames.Any(f =>
                f.GetMethod() is { IsConstructor: true } &&
                typeof(MultiSensorBase).IsAssignableFrom(f.GetMethod().DeclaringType)
            );

            if (!calledFromConstructor)
                UnityEngine.Debug.LogWarning("Multi sensor registration must be added from the constructor of the sensor, not the Created method.");
#endif
        }
    }

    public class GlobalSensor : IGlobalSensor
    {
        public Action<IWorldData> SenseMethod;
        public Type Key { get; set; }

        public void Created() { }

        public Type[] GetKeys() => new[] { this.Key };

        public void Sense(IWorldData data) => this.SenseMethod(data);
    }

    public class LocalSensor : ILocalSensor
    {
        public Action<IWorldData, IActionReceiver, IComponentReference> SenseMethod;
        public Type Key { get; set; }

        public Type[] GetKeys() => new[] { this.Key };

        public void Created() { }

        public void Update() { }

        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references) => this.SenseMethod(data, agent, references);
    }
}
