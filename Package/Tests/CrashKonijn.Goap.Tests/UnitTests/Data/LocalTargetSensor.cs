using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

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

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            return default;
        }
    }
}