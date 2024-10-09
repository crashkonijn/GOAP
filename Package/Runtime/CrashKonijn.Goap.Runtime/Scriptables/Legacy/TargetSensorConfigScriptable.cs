using System;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use CapabilityConfigs instead!")]
    public class TargetSensorConfigScriptable : ScriptableObject, ITargetSensorConfig
    {
        [TargetSensor]
        public string classType;

        public TargetKeyScriptable key;

        public string Name => this.name;

        public ITargetKey Key => this.key;

        public string ClassType
        {
            get => this.classType;
            set => this.classType = value;
        }
    }
}