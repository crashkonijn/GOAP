using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public struct AgentDebugGraph
    {
        public List<IGoalBase> Goals { get; set; }
        public List<IActionBase> Actions { get; set; }
        public IGoapConfig Config { get; set; }
    }
}