using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-TransformSensor")]
    public class AgentSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            return new TransformTarget(agent.transform);
        }
    }
}