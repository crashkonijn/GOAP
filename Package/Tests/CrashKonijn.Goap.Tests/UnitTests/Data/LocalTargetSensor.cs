using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class LocalTargetSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references)
        {
            return default;
        }
    }
}