using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoalBase : IAction
    {
        public GoalConfig Config { get; }

        public int GetCost(IWorldData data);
        public void SetConfig(GoalConfig config);
    }
}