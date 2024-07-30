using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IGoalRequest
    {
        IGoal[] Goals { get; }
    }

    public interface IGoalResult
    {
        IGoal Goal { get; }
        IConnectable[] Plan { get; }
        IGoapAction Action { get; }
    }
}