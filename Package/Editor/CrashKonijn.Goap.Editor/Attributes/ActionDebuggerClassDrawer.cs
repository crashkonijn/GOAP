using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Interfaces;
using UnityEditor;

namespace CrashKonijn.Goap.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(ActionDebuggerClassAttribute))]
    public class ActionDebuggerClassDrawer : ClassDrawerBase<IAgentDebugger>
    {
    }
}