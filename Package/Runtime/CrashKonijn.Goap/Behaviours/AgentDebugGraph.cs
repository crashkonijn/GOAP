using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public struct AgentDebugGraph
    {
        public HashSet<IGoalBase> Goals { get; set; }
        public HashSet<IActionBase> Actions { get; set; }
        public GoapConfig Config { get; set; }
    }
}