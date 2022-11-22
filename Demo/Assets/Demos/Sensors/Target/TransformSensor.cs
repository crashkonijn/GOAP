using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class TransformSensor : LocalTargetSensorBase
    {
        public override ITarget Sense(IMonoAgent agent)
        {
            return new TransformTarget(agent.transform);
        }
    }
}