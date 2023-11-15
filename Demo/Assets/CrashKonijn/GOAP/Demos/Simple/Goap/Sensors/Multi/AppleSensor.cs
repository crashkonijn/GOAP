using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Multi
{
    [GoapId("Simple-AppleSensor")]
    public class AppleSensor : IMultiSensor
    {
        public IMultiSensorConfig Config { get; }
        public void SetConfig(IMultiSensorConfig config)
        {
        }

        public void Created()
        {
        }

        public void Sense(IWorldData data)
        {
        }

        public void Update()
        {
        }

        public void Sense(IWorldData data, IMonoAgent agent, IComponentReference references)
        {
        }
    }
}