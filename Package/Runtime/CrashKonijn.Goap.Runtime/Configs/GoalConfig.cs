using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class GoalConfig : IGoalConfig, IClassCallbackConfig
    {
        public GoalConfig() { }

        public GoalConfig(Type type)
        {
            this.Name = type.Name;
            this.ClassType = type.AssemblyQualifiedName;
        }

        public string Name { get; set; }
        public string ClassType { get; set; }
        public float BaseCost { get; set; }
        public List<ICondition> Conditions { get; set; } = new();
        public Action<object> Callback { get; set; }

        public static GoalConfig Create<TGoal>()
            where TGoal : IGoal
        {
            return new GoalConfig(typeof(TGoal));
        }
    }
}
