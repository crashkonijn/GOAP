using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.Target
{
    public class TransformSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references)
        {
            return new TransformTarget(agent.Transform);
        }
    }
}