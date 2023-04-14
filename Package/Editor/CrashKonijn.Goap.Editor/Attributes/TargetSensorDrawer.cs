using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Interfaces;
using UnityEditor;

namespace CrashKonijn.Goap.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(TargetSensorAttribute))]
    public class TargetSensorDrawer : ClassDrawerBase<ITargetSensor>
    {
    }
}