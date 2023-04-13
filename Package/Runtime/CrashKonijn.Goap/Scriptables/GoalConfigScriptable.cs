using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Serializables;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoalConfig")]
    public class GoalConfigScriptable : ScriptableObject, IGoalConfig
    {
        [GoalClass]
        public string classType;
        public int baseCost = 1;
        public List<SerializableCondition> conditions;

        public string Name => this.name;

        public List<ICondition> Conditions => this.conditions.Cast<ICondition>().ToList();

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