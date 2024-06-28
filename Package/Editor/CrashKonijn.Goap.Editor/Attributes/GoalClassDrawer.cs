using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;

namespace CrashKonijn.Goap.Editor
{
    [CustomPropertyDrawer(typeof(GoalClassAttribute))]
    public class GoalClassDrawer : ClassDrawerBase<IGoal>
    {
    }
}