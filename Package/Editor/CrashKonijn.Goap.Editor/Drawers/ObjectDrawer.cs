using System.Linq;
using System.Reflection;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ObjectDrawer : VisualElement
    {
        public ObjectDrawer(object obj)
        {
            if (obj is null)
                return;
            
            var properties = obj.GetType().GetProperties();
            
            var label = new Label();
            label.text = this.GetLabelText(properties, obj);
            this.Add(label);

            this.schedule.Execute(() =>
            {
                label.text = this.GetLabelText(properties, obj);
            }).Every(33);
        }

        private string GetLabelText(PropertyInfo[] properties, object obj)
        {
            return string.Join("\n", properties.Select(x =>
            {
                var value = x.GetValue(obj);
                return $"{x.Name}: {this.GetValueString(value)}";
            }));
        }

        private string GetValueString(object value)
        {
            if (value == null)
                return "null";
            
            if (value is TransformTarget transformTarget)
            {
                if (transformTarget.Transform == null)
                    return "null";
                
                return transformTarget.Transform.name;
            }
            
            if (value is PositionTarget positionTarget)
                return positionTarget.Position.ToString();
            
            if (value is MonoBehaviour monoBehaviour)
            {
                if (monoBehaviour == null)
                    return "null";
                
                return monoBehaviour.name;
            }

            if (value is ScriptableObject scriptableObject)
            {
                if (scriptableObject == null)
                    return "null";

                return scriptableObject.name;
            }

            return value.ToString();
        }
    }
}