using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;

namespace CrashKonijn.Goap.Editor
{
    [CustomPropertyDrawer(typeof(ActionClassAttribute))]
    public class ActionClassDrawer : ClassDrawerBase<IAction>
    {
    }
}