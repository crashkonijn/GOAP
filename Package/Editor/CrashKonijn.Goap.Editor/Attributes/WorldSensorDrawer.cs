using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;

namespace CrashKonijn.Goap.Editor
{
    [CustomPropertyDrawer(typeof(WorldSensorAttribute))]
    public class WorldSensorDrawer : ClassDrawerBase<IWorldSensor>
    {
    }
}