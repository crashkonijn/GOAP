using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Goals
{
    public class PickupItemGoal<THoldable> : GoalBase
        where THoldable : IHoldable
    {
    }
}