using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Scriptables;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
{
    [Serializable]
    public class ActionConfig
    {
        [HideInInspector]
        public string actionClass;
        public int baseCost = 1;
        public TargetKeyScriptable target;
        public float inRange = 0.1f;
        
        public List<Condition> conditions;
        public List<Effect> effects;
    }
}