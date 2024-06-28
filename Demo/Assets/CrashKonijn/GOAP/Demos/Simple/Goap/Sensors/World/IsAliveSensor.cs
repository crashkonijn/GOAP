using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.World
{
    [GoapId("Simple-IsAliveSensor")]
    public class IsAliveSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return true;
        }
    }
}