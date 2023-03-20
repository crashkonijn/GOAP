using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class LocalWorldSensor : LocalWorldSensorBase
    {
        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent)
        {
            return default;
        }
    }
}