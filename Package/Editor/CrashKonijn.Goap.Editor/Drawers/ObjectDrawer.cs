using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Drawers
{
    public class ObjectDrawer : VisualElement
    {
        public ObjectDrawer(object obj)
        {
            if (obj is null)
                return;
            
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                
                this.Add(new Label($"{property.Name}: {this.GetValueString(value)}"));
            }
        }

        private string GetValueString(object value)
        {
            if (value is null)
                return "null";
            
            if (value is TransformTarget transformTarget)
                return transformTarget.Transform.name;
            
            if (value is PositionTarget positionTarget)
                return positionTarget.Position.ToString();
            
            if (value is MonoBehaviour monoBehaviour)
                return monoBehaviour.name;
            
            if (value is ScriptableObject scriptableObject)
                return scriptableObject.name;

            return value.ToString();
        }
    }
}