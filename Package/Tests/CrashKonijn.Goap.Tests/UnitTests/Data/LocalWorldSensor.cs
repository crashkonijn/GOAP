using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class LocalWorldSensor : LocalWorldSensorBase
    {
        public override bool Sense(IMonoAgent agent)
        {
            return default;
        }
    }
}