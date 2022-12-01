using System.Collections.Generic;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoalConfig")]
    public class GoalConfigScriptable : ScriptableObject, IGoalConfig
    {
        [GoalClass]
        public string classType;
        public int baseCost = 1;
        public List<Condition> conditions;

        public string Name => this.name;
        
        public List<Condition> Conditions
        {
            get => this.conditions;
            set => this.conditions = value;
        }

        public int BaseCost
        {
            get => this.baseCost;
            set => this.baseCost = value;
        }

        public string ClassType
        {
            get => this.classType;
            set => this.classType = value;
        }
    }
}