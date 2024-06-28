using System;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IMultiSensor : IHasConfig<IMultiSensorConfig>, ILocalSensor, IGlobalSensor
    {
        string[] GetSensors();
        void Sense(IWorldData data, Type[] keys);
        void Sense(IWorldData data, IActionReceiver agent, IComponentReference references, Type[] keys);
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