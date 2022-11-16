﻿using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class ClosestTree : LocalTargetSensorBase
    {
        public override ITarget Sense(Agent agent)
        {
            return new TransformTarget(GameObject.FindObjectsOfType<TreeBehaviour>().Closest(agent.transform.position).transform);
        }
    }
}