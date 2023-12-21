using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEditor;

namespace CrashKonijn.Goap.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(ActionClassAttribute))]
    public class ActionClassDrawer : ClassDrawerBase<IAction>
    {
    }
}