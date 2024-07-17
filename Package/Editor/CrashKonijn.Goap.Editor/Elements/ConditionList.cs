using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ConditionList : ListElementBase<CapabilityCondition, CapabilityConditionElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public ConditionList(SerializedProperty serializedProperty, CapabilityConfigScriptable scriptable, GeneratorScriptable generator, List<CapabilityCondition> conditions) : base(serializedProperty.FindPropertyRelative("conditions"), conditions)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }


        protected override CapabilityConditionElement CreateListItem(SerializedProperty property, CapabilityCondition item)
        {
            return new CapabilityConditionElement(this.scriptable, this.generator);
        }

        protected override void BindListItem(SerializedProperty property,  CapabilityConditionElement element, CapabilityCondition item, int index)
        {
            element.Foldout.text = item.ToString();
            
            element.WorldKeyField.Bind(this.scriptable, item.worldKey, this.generator.GetWorldKeys().ToArray(), classRef =>
            {
                element.Foldout.text = item.ToString();
            });
            
            element.ComparisonField.value = item.comparison;
            element.ComparisonField.RegisterValueChangedCallback(evt =>
            {
                item.comparison = (Comparison) evt.newValue;
                element.Foldout.text = item.ToString();
                EditorUtility.SetDirty(this.scriptable);
            });
            
            element.AmountField.value = item.amount;
            element.AmountField.RegisterValueChangedCallback(evt =>
            {
                item.amount = evt.newValue;
                element.Foldout.text = item.ToString();
                EditorUtility.SetDirty(this.scriptable);
            });
        }
    }
}