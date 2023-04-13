using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/WorldKey")]
    public class WorldKeyScriptable : ScriptableObject, IWorldKey
    {
        public string Name => this.name;
    }
}