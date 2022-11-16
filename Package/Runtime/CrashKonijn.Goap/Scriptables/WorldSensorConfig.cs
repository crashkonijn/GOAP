using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/WorldSensorConfig")]
    public class WorldSensorConfig : ScriptableObject
    {
        [HideInInspector]
        public string sensorClass;

        public WorldKey key;
    }
}