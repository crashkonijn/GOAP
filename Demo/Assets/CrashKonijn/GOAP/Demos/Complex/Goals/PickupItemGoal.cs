using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Goals
{
    public class PickupItemGoal<THoldable> : GoalBase
        where THoldable : IHoldable
    {
    }
}