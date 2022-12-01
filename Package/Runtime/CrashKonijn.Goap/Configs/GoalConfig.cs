using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    [Serializable]
    public class GoalConfig<TGoal> : IGoalConfig
        where TGoal : IGoalBase
    {
        public GoalConfig()
        {
            this.Name = typeof(TGoal).Name;
            this.ClassType = typeof(TGoal).AssemblyQualifiedName;
        }
        
        public GoalConfig(string name)
        {
            this.Name = name;
            this.ClassType = typeof(TGoal).AssemblyQualifiedName;
        }
        
        public string Name { get; }
        public string ClassType { get; }
        public int BaseCost { get; set; }
        public List<Condition> Conditions { get; set; } = new();
    }
}