using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

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

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            // It's always the same target, return it again if it already exists
            if (target != null)
                return target;
            
            return new TransformTarget(agent.Transform);
        }
    }
}