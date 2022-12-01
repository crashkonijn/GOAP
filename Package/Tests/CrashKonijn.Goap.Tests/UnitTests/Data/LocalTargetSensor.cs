using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace Packages.LamosInteractive.Goap.Unity.Tests.UnitTests.Data
{
    public class LocalTargetSensor : LocalTargetSensorBase
    {
        public override ITarget Sense(IMonoAgent agent)
        {
            return default;
        }
    }
}