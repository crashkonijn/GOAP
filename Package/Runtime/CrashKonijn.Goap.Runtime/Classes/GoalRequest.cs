using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoalRequest : IGoalRequest
    {
        public IGoal[] Goals { get; set; }
        public string Key { get; set; }
    }
    
    public class GoalResult : IGoalResult
    {
        public IGoal Goal { get; set; }
        public IConnectable[] Plan { get; set; }
        public IGoapAction Action { get; set; }
    }
}