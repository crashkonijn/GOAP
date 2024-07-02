using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [CreateAssetMenu(menuName = "Goap/TargetKey")]
    public class TargetKeyScriptable : ScriptableObject, ITargetKey
    {
        public string Name => this.name;
    }
}