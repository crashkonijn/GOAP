using CrashKonijn.Goap.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Goals
{
    public class CreateItemGoal<THoldable> : GoalBase
        where THoldable : ICreatable
    {
    }
}