using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Sensors;

namespace Packages.LamosInteractive.Goap.Unity.Tests.UnitTests.Data
{
    public class LocalWorldSensor : LocalWorldSensorBase
    {
        public override bool Sense(IMonoAgent agent)
        {
            return default;
        }
    }
}