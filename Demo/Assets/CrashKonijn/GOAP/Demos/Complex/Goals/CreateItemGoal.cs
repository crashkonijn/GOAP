using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Goals
{
    public class CreateItemGoal<THoldable> : GoalBase
        where THoldable : ICreatable
    {
    }
}