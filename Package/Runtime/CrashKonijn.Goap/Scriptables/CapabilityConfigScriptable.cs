using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/CapabilityConfig")]
    public class CapabilityConfigScriptable : ScriptableObject
    {
        public List<BehaviourGoal> goals = new();
        public List<BehaviourAction> actions = new();
        public List<BehaviourWorldSensor> worldSensors = new();
        public List<BehaviourTargetSensor> targetSensors = new();
        public List<BehaviourMultiSensor> multiSensors = new();
    }

    [Serializable]
    public class BehaviourGoal
    {
        public ClassRef goal = new();
        
        public List<BehaviourCondition> conditions = new();
    }
    
    [Serializable]
    public class BehaviourAction
    {
        public ClassRef action = new();
        public int baseCost = 1;
        public ClassRef target = new();
        public float inRange = 0.1f;
        public ActionMoveMode moveMode;
        public List<BehaviourCondition> conditions = new();
        public List<BehaviourEffect> effects = new();
    }
    
    [Serializable]
    public class BehaviourCondition
    {
        // public string name => this.ToString();
        
        public ClassRef worldKey = new();
        public Comparison comparison;
        public int amount;

        public BehaviourCondition()
        {
            
        }
        
        public BehaviourCondition(string data)
        {
            var split = data.Split(' ');
            this.worldKey.name = split[0];
            this.comparison = split[1].FromName();
            this.amount = int.Parse(split[2]);
        }

        public override string ToString() => $"{this.worldKey.name} {this.comparison.ToName()} {this.amount}";
    }
    
    [Serializable]
    public class BehaviourEffect
    {
        // public string name => this.ToString();
        
        public ClassRef worldKey = new();
        public EffectType effect;

        public override string ToString() => $"{this.worldKey.name}{this.effect.ToName()}";
    }

    [Serializable]
    public class BehaviourWorldSensor : BehaviourSensor
    {
        public ClassRef worldKey = new();

        public override string ToString() => $"{this.sensor.name} ({this.worldKey.name})";
    }

    [Serializable]
    public class BehaviourTargetSensor : BehaviourSensor
    {
        public ClassRef targetKey = new();

        public override string ToString() => $"{this.sensor.name} ({this.targetKey.name})";
    }

    [Serializable]
    public class BehaviourMultiSensor : BehaviourSensor
    {
        public override string ToString() => this.sensor.name;
    }

    [Serializable]
    public abstract class BehaviourSensor
    {
        public ClassRef sensor = new();
    }

    [Serializable]
    public class ClassRef
    {
        public string name;
        public string id;
    }
    
    public enum ClassRefStatus
    {
        None,
        Name,
        Id,
        Full
    }
}