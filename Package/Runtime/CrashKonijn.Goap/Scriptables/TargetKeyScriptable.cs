using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/TargetKey")]
    public class TargetKeyScriptable : ScriptableObject, ITargetKey
    {
        public string Name => this.name;
    }
}