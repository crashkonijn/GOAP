using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [CreateAssetMenu(menuName = "Goap/WorldKey")]
    public class WorldKeyScriptable : ScriptableObject, IWorldKey
    {
        public string Name => this.name;
    }
}