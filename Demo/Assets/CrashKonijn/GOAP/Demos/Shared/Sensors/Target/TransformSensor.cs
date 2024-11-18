using CrashKonijn.Agent.Core;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Runtime;

namespace Demos.Shared.Sensors.Target
{
    public class TransformSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            return new TransformTarget(agent.Transform);
        }
    }
}