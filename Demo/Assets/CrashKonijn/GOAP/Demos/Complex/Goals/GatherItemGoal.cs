using CrashKonijn.Goap.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Goals
{
    public class GatherItemGoal<TGatherable> : GoalBase
        where TGatherable : IGatherable
    {
    }
}