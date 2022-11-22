using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class ClosestTree : LocalTargetSensorBase
    {
        public override ITarget Sense(IMonoAgent agent)
        {
            return new TransformTarget(GameObject.FindObjectsOfType<TreeBehaviour>().Closest(agent.transform.position).transform);
        }
    }
}