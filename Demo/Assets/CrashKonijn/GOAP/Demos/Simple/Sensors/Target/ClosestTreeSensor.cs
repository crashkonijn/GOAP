using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Simple.Behaviours;
using UnityEngine;

namespace Demos.Simple.Sensors.Target
{
    public class ClosestTreeSensor : LocalTargetSensorBase
    {
        private TreeBehaviour[] trees;

        public override void Created()
        {            
            this.trees = GameObject.FindObjectsOfType<TreeBehaviour>();
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