using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class WorldSensorBase : MonoBehaviour
    {
        [SerializeField]
        private IWorldKey keyScriptable;

        public IWorldKey KeyScriptable => this.keyScriptable;
    }
}