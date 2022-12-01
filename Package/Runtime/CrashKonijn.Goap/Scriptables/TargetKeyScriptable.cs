using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/TargetKey")]
    public class TargetKeyScriptable : ScriptableObject, ITargetKey, IEffect, ICondition
    {
        public string Name => this.name;
    }
}