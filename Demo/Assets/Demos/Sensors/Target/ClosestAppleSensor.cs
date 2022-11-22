using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class ClosestAppleSensor : LocalTargetSensorBase
    {
        public override ITarget Sense(IMonoAgent agent)
        {
            return new TransformTarget(GameObject.FindObjectsOfType<AppleBehaviour>().Where(x => x.GetComponent<Renderer>().enabled).Closest(agent.transform.position).transform);
        }
    }
}