using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class LocalWorldSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return default;
        }
    }
}