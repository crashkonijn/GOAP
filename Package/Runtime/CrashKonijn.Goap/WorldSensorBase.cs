using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap
{
    public abstract class WorldSensorBase : MonoBehaviour
    {
        [SerializeField]
        private IWorldKey keyScriptable;

        public IWorldKey KeyScriptable => this.keyScriptable;
    }
}