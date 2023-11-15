using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentDebugGraph
    {
        List<IGoal> Goals { get; set; }
        List<IAction> Actions { get; set; }
        IGoapConfig Config { get; set; }
    }
}