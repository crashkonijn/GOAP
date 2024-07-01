using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IGoalRequest
    {
        public IGoal[] Goals { get; }
    }
}