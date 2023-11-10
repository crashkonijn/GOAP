using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Resolver;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/BehaviourConfig")]
    public class BehaviourConfigScriptable : ScriptableObject
    {
        public List<BehaviourGoal> goals = new();
        public List<BehaviourAction> actions = new();
        public List<BehaviourWorldSensorConfig> worldSensors = new();
        public List<BehaviourTargetSensorConfig> targetSensors = new();
    }

    [Serializable]
    public class BehaviourGoal
    {
        public string name;
        
        public List<BehaviourCondition> conditions = new();
    }
    
    [Serializable]
    public class BehaviourAction
    {
        public string name;
        public int baseCost = 1;
        public string target;
        public float inRange = 0.1f;
        public ActionMoveMode moveMode;
        public List<BehaviourCondition> conditions;
        public List<BehaviourEffect> effects;
    }
    
    [Serializable]
    public class BehaviourCondition
    {
        public string worldKey;
        public Comparison comparison;
        public int amount;
        
        public BehaviourCondition(string data)
        {
            var split = data.Split(' ');
            this.worldKey = split[0];
            this.comparison = split[1].FromName();
            this.amount = int.Parse(split[2]);
        }

        public override string ToString() => $"{this.worldKey} {this.comparison.ToName()} {this.amount}";
    }
    
    [Serializable]
    public class BehaviourEffect
    {
        public string worldKey;
        public EffectType effect;

        public override string ToString() => $"{this.worldKey}{this.effect.ToName()}";
    }

    [Serializable]
    public class BehaviourWorldSensorConfig
    {
        public string name;
        public string worldKey;
    }

    [Serializable]
    public class BehaviourTargetSensorConfig
    {
        public string name;
        public string targetKey;
    }
}