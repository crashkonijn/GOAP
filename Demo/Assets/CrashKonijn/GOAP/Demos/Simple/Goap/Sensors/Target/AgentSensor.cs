using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-TransformSensor")]
    public class AgentSensor : LocalTargetSensorBase
    {
        public override ISensorTimer Timer { get; } = SensorTimer.Once;

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