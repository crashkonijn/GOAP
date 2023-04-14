using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Interfaces;
using UnityEditor;

namespace CrashKonijn.Goap.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(ActionClassAttribute))]
    public class ActionClassDrawer : ClassDrawerBase<IActionBase>
    {
    }
}