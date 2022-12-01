using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(WorldSensorConfigScriptable))]
    public class WorldSensorConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (WorldSensorConfigScriptable) this.target;
            
            base.OnInspectorGUI();

            config.ClassType = this.DrawTypeSelect(config.ClassType, this.GetAllOfType<IWorldSensor>());
        }

        private string DrawTypeSelect(string currentValue, List<string> options)
        {
            var index = options.FindIndex(x => x == currentValue);

            index = EditorGUILayout.Popup("ActionClass", index, options.ToArray());

            if (index == -1)
                return "";

            return options[index];
        }

        private List<string> GetAllOfType<TType>()
        {
            var type = typeof(TType);
            
            return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .Select(x => x.AssemblyQualifiedName).ToList();
        }
    }
}