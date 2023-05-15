using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public delegate void ActionDelegate(IActionBase action);
    public delegate void GoalDelegate(IGoalBase goal);
    public delegate void TargetDelegate(ITarget target);
    public delegate void TargetRangeDelegate(ITarget target, bool inRange);
}