using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ActionConfig : IActionConfig, IClassCallbackConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public IActionProperties Properties { get; set; }
        public float BaseCost { get; set; }
        public ITargetKey Target { get; set; }
        public float StoppingDistance { get; set; }
        public bool ValidateTarget { get; set; }
        public bool RequiresTarget { get; set; }
        public bool ValidateConditions { get; set; }
        public ICondition[] Conditions { get; set; }
        public IEffect[] Effects { get; set; }
        public ActionMoveMode MoveMode { get; set; } = ActionMoveMode.MoveBeforePerforming;
        public Action<object> Callback { get; set; }
    }
}
