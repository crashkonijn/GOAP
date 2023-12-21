using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Goals
{
    public class GatherItemGoal<TGatherable> : GoalBase
        where TGatherable : IGatherable
    {
    }
}