using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Goap.Editor
{
#pragma warning disable CS0618 // Type or member is obsolete
    [CustomPropertyDrawer(typeof(SerializableEffect))]
#pragma warning restore CS0618 // Type or member is obsolete
    public class SerializableEffectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw the fields of MyData as desired.
            Rect propertyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative("worldKey"), new GUIContent("World Key"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var increase = property.FindPropertyRelative("<Increase>k__BackingField");
            var type = increase.boolValue ? EffectType.Increase : EffectType.Decrease;
            
            type = (EffectType) EditorGUI.EnumPopup(position, new GUIContent("Type"), type);
            
            increase.boolValue = type == EffectType.Increase;

            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Return the height of the property field(s) based on how you render them.
            // You might need to calculate this dynamically based on your layout.
            return (EditorGUIUtility.singleLineHeight * 2) + EditorGUIUtility.standardVerticalSpacing; // Adjust as needed.
        }
    }
}