using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using Debug = UnityEngine.Debug;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class MultiSensorBase : IMultiSensor
    {
        public IMultiSensorConfig Config { get; private set; }

        public Dictionary<Type, ILocalSensor> LocalSensors { get; private set; } = new();
        public Dictionary<Type, IGlobalSensor> GlobalSensors { get; private set; } = new();

        /// <summary>
        ///     Sets the configuration for the multi-sensor.
        /// </summary>
        /// <param name="config">The multi-sensor configuration.</param>
        public void SetConfig(IMultiSensorConfig config)
        {
            this.Config = config;
        }

        public Type Key { get; set; }

        /// <summary>
        ///     Called when the sensor is created. Don't use this method to add sensors, use the constructor instead.
        /// </summary>
        public abstract void Created();

        /// <summary>
        ///     Called when the sensor should update. Use this for caching or other performance optimizations.
        /// </summary>
        public abstract void Update();

        /// <summary>
        ///     Gets the keys of the sensors.
        /// </summary>
        /// <returns>An array of sensor keys.</returns>
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

        /// <summary>
        ///     Senses the world data using global sensors.
        /// </summary>
        /// <param name="data">The world data.</param>
        public void Sense(IWorldData data)
        {
            foreach (var sensor in this.GlobalSensors.Values)
            {
                sensor.Sense(data);
            }
        }

        /// <summary>
        ///     Senses the world data using specific global sensors.
        /// </summary>
        /// <param name="data">The world data.</param>
        /// <param name="keys">The keys of the sensors to use.</param>
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

        /// <summary>
        ///     Senses the world data using local sensors.
        /// </summary>
        /// <param name="data">The world data.</param>
        /// <param name="agent">The action receiver.</param>
        /// <param name="references">The component references.</param>
        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references)
        {
            foreach (var sensor in this.LocalSensors.Values)
            {
                sensor.Sense(data, agent, references);
            }
        }

        /// <summary>
        ///     Senses the world data using specific local sensors.
        /// </summary>
        /// <param name="data">The world data.</param>
        /// <param name="agent">The action receiver.</param>
        /// <param name="references">The component references.</param>
        /// <param name="keys">The keys of the sensors to use.</param>
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

        /// <summary>
        ///     Adds a local world sensor.
        /// </summary>
        /// <typeparam name="TKey">The type of the world key.</typeparam>
        /// <param name="sense">The sense function.</param>
        /// <param name="timer">The sensor timer.</param>
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

        /// <summary>
        ///     Adds a global world sensor.
        /// </summary>
        /// <typeparam name="TKey">The type of the world key.</typeparam>
        /// <param name="sense">The sense function.</param>
        /// <param name="timer">The sensor timer.</param>
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

        /// <summary>
        ///     Adds a local target sensor.
        /// </summary>
        /// <typeparam name="TKey">The type of the target key.</typeparam>
        /// <param name="sense">The sense function.</param>
        /// <param name="timer">The sensor timer.</param>
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

        /// <summary>
        ///     Adds a global target sensor.
        /// </summary>
        /// <typeparam name="TKey">The type of the target key.</typeparam>
        /// <param name="sense">The sense function.</param>
        /// <param name="timer">The sensor timer.</param>
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

        /// <summary>
        ///     Gets the names of the sensors.
        /// </summary>
        /// <returns>An array of sensor names.</returns>
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
                Debug.LogWarning("Multi sensor registration must be added from the constructor of the sensor, not the Created method.");
#endif
        }
    }
}
