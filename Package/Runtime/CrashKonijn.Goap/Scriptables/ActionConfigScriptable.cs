using System.Collections.Generic;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Serializables;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/ActionConfig")]
    public class ActionConfigScriptable : ScriptableObject, IActionConfig
    {
        [ActionClass]
        public string actionClass;
        public int baseCost = 1;
        public TargetKeyScriptable target;
        public float inRange = 0.1f;
        
        public List<SerializableCondition> conditions;
        public List<SerializableEffect> effects;
        
        public string ClassType => this.actionClass;
        public int BaseCost => this.baseCost;
        public ITargetKey Target => this.target;
        public float InRange => this.inRange;

        public ICondition[] Conditions => this.conditions.ToArray();
        public IEffect[] Effects => this.effects.ToArray();

        public string Name => this.name;
    }
}