using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Core
{
    public delegate void ActionDelegate(IAction action);
    public delegate void GoalDelegate(IGoal goal);
    public delegate void TargetDelegate(ITarget target);
    public delegate void TargetRangeDelegate(ITarget target, bool inRange);
}