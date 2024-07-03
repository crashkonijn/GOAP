using System;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use CapabilityConfigs instead!")]
    public class WorldSensorConfigScriptable : ScriptableObject, IWorldSensorConfig
    {
        [WorldSensor]
        public string classType;

        public WorldKeyScriptable key;

        public string Name => this.name;

        public string ClassType
        {
            get => this.classType;
            set => this.classType = value;
        }

        public IWorldKey Key => this.key;
    }
}