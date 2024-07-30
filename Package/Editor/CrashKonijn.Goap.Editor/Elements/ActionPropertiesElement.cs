using System;
using System.Reflection;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ActionPropertiesElement : VisualElement
    {
        private readonly SerializedObject serializedObject;
        private PropertyField props;

        public ActionPropertiesElement(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }

        public void Bind(SerializedProperty property, CapabilityAction action, Script[] actions)
        {
            this.validate(action, actions);
                
            this.Render(action.properties, property.FindPropertyRelative("properties"));
        }

        private void Render(IActionProperties props, SerializedProperty property)
        {
            this.Clear();
            
            if (props == null)
                return;
            
            var type = props.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            this.Add(new Label("Action props"));
            this.Add(new Card((card) =>
            {
                if (properties.Length == 0 && fields.Length == 0)
                {
                    card.Add(new Label("No properties"));
                    return;
                }
                
                foreach (var propertyInfo in properties)
                {
                    this.AddProp(card, property.FindPropertyRelative(propertyInfo.Name));
                }
            
                foreach (var propertyInfo in fields)
                {
                    this.AddProp(card, property.FindPropertyRelative(propertyInfo.Name));
                }
            }));
        }

        private void AddProp(Card card, SerializedProperty prop)
        {
            var field = new PropertyField(prop);
            field.BindProperty(prop);
            field.Bind(this.serializedObject);
            card.Add(field);
        }

        private void validate(CapabilityAction action, Script[] actions)
        {
            var (status, script) = action.action.GetMatch(actions);

            if (status != ClassRefStatus.Full)
                return;
            
            var type = this.GetPropertiesType(script);
            
            if (type == null)
                return;
            

            if (action.properties?.GetType() == type)
            {
                return;
            }
            
            action.properties = (IActionProperties) Activator.CreateInstance(type);
        }

        private Type GetPropertiesType(Script script)
        {
            if (script == null)
                return null;
            
            var type = script.Type;
            var baseType = type.BaseType;
            
            if (baseType == null)
                return null;
            
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(GoapActionBase<,>)) 
                return baseType.GetGenericArguments()[1];
            
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(GoapActionBase<>)) 
                return typeof(EmptyActionProperties);
            
            return null;
        }
    }
}