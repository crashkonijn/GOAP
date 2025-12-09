using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.World
{
    public class LowHungerSensor : LocalWorldSensorBase
    {
        public override ISensorTimer Timer { get; } = SensorTimer.Once;

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return  30;
        }
    }
}
