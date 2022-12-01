using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/TargetSensorConfig")]
    public class TargetSensorConfigScriptable : ScriptableObject, ITargetSensorConfig
    {
        [HideInInspector]
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