using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace Demos.Simple.Sensors.Target
{
    public class WanderTargetSensor : LocalTargetSensorBase
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var random = this.GetRandomPosition(agent);
            
            if (target is PositionTarget positionTarget)
            {
                return positionTarget.SetPosition(random);
            }
            
            return new PositionTarget(random);
        }

        private Vector3 GetRandomPosition(IActionReceiver agent)
        {
            while (true)
            {
                var random = Random.insideUnitCircle * 5f;
                var position = agent.Transform.position + new Vector3(random.x, 0f, random.y);

                if (position.x > -Bounds.x && position.x < Bounds.x && position.z > -Bounds.y && position.z < Bounds.y) return position;
            }
        }
    }
}