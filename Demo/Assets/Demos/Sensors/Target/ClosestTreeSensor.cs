using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class ClosestTreeSensor : LocalTargetSensorBase
    {
        private readonly TreeBehaviour[] trees;

        public ClosestTreeSensor()
        {
            this.trees = GameObject.FindObjectsOfType<TreeBehaviour>();
        }
        
        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent)
        {
            return new TransformTarget(this.trees.Closest(agent.transform.position).transform);
        }
    }
}