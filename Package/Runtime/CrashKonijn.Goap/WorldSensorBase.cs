using CrashKonijn.Goap.Scriptables;
using UnityEngine;

namespace CrashKonijn.Goap
{
    public abstract class WorldSensorBase : MonoBehaviour
    {
        [SerializeField]
        private WorldKey key;

        public WorldKey Key => this.key;
    }
}