using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Goap.Editor
{
    public abstract class ClassDrawerBase<TClassType> : PropertyDrawer
    {
        private List<string> types = new();

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = this.DrawTypeSelect(position, property.stringValue, this.GetAllOfType<TClassType>());
        }

        private string DrawTypeSelect(Rect position, string currentValue, List<string> options)
        {
            var index = options.FindIndex(x => x == currentValue);

            index = EditorGUI.Popup(position, "Class", index, options.ToArray());

            if (index == -1)
                return "";

            return options[index];
        }

        private List<string> GetAllOfType<TType>()
        {
            if (!this.types.Any())
            {
                this.types = this.LoadAllOfType<TType>();
            }

            return this.types;
        }

        private List<string> LoadAllOfType<TType>()
        {
            var type = typeof(TType);

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !a.FullName.ToLower().Contains("test"))
                .SelectMany(a => a.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(x => x.AssemblyQualifiedName).ToList();
        }
    }
}