using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/TargetSensorConfig")]
    public class TargetSensorConfig : ScriptableObject
    {
        [HideInInspector]
        public string sensorClass;

        public TargetKey key;
    }
}