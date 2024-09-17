using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Runtime;
using Demos;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-ClosestTreeSensor")]
    public class ClosestTreeSensor : LocalTargetSensorBase
    {
        private TreeBehaviour[] trees;

        public override void Created()
        {            
            this.trees = Compatibility.FindObjectsOfType<TreeBehaviour>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            return new TransformTarget(this.trees.Closest(agent.Transform.position).transform);
        }
    }
}