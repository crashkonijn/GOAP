using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class WanderTargetSensor : LocalTargetSensorBase
    {
        public override ITarget Sense(IMonoAgent agent)
        {
            var random = Random.insideUnitCircle * 10f;
            return new PositionTarget(agent.transform.position + new Vector3(random.x, 0f, random.y) );
        }
    }
}