using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IMultiSensor : IHasConfig<IMultiSensorConfig>, ILocalSensor, IGlobalSensor
    {
        string[] GetSensors();
        Dictionary<Type, ILocalSensor> LocalSensors { get; }
        Dictionary<Type, IGlobalSensor> GlobalSensors { get; }
    }

    public interface ISensor
    {
        public void Created();
        public Type[] GetKeys();
    }

    public interface ILocalSensor : ISensor
    {
        public void Update();
        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references);
    }

    public interface IGlobalSensor : ISensor
    {
        public void Sense(IWorldData data);
    }
}