using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use CapabilityConfigs instead!")]
    public class GoalConfigScriptable : ScriptableObject, IGoalConfig
    {
        [GoalClass]
        public string classType;

        public float baseCost = 1;
        public List<SerializableCondition> conditions;

        public string Name => this.name;

        public List<ICondition> Conditions => this.conditions.Cast<ICondition>().ToList();

        public float BaseCost
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
