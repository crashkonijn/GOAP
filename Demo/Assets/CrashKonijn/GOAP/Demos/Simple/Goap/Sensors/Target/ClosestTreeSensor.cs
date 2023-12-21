using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;
using CrashKonijn.Goap.Sensors;
using Demos;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-ClosestTreeSensor")]
    public class ClosestTreeSensor : LocalTargetSensorBase
    {
        private TreeBehaviour[] trees;

        public override void Created()
        {            
            this.trees = Object.FindObjectsOfType<TreeBehaviour>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            return new TransformTarget(this.trees.Closest(agent.transform.position).transform);
        }
    }
}