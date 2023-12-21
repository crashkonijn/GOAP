using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public struct AgentDebugGraph : IAgentDebugGraph
    {
        public List<IGoal> Goals { get; set; }
        public List<IAction> Actions { get; set; }
        public IGoapConfig Config { get; set; }
    }
}