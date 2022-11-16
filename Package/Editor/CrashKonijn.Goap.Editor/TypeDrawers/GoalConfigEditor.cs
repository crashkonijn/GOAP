using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoalConfig))]
    public class GoalConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (GoalConfig) this.target;
            
            base.OnInspectorGUI();

            config.goalClass = this.DrawTypeSelect(config.goalClass, this.GetAllOfType<IGoalBase>());
        }

        private string DrawTypeSelect(string currentValue, List<string> options)
        {
            var index = options.FindIndex(x => x == currentValue);

            index = EditorGUILayout.Popup("GoalClass", index, options.ToArray());

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