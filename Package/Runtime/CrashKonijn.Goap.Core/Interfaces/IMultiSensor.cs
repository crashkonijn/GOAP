namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IMultiSensor : IHasConfig<IMultiSensorConfig>, ILocalSensor, IGlobalSensor
    {
        string[] GetSensors();
    }

    public interface ISensor
    {
        public void Created();
    }

    public interface ILocalSensor : ISensor
    {
        public void Update();
        public void Sense(IWorldData data, IMonoAgent agent, IComponentReference references);
    }

    public interface IGlobalSensor : ISensor
    {
        public void Sense(IWorldData data);
    }
}