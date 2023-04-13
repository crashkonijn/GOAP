using System.Collections.Generic;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Serializables;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/ActionConfig")]
    public class ActionConfigScriptable : ScriptableObject, IActionConfig
    {
        [ActionClass]
        public string actionClass;
        public TargetKeyScriptable target;
        
        public List<SerializableCondition> conditions;
        public List<SerializableEffect> effects;
        
        public string ClassType
        {
            get => this.actionClass;
            set => this.actionClass = value;
        }
        
        [field:SerializeField]
        public int BaseCost { get; set; } = 1;
        
        [field:SerializeField]
        public float InRange { get; set; } = 0.1f;
        
        public ITargetKey Target => this.target;

        public ICondition[] Conditions => this.conditions.ToArray();
        public IEffect[] Effects => this.effects.ToArray();

        public string Name => this.name;
    }
}