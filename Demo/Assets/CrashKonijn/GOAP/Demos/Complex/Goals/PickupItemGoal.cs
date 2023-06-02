using CrashKonijn.Goap.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Goals
{
    public class PickupItemGoal<THoldable> : GoalBase
        where THoldable : IHoldable
    {
    }
}