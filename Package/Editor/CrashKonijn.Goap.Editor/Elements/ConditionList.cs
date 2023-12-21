using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class ConditionList : ListElementBase<BehaviourCondition, CapabilityConditionElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public ConditionList(CapabilityConfigScriptable scriptable, GeneratorScriptable generator, List<BehaviourCondition> conditions) : base(conditions)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }


        protected override CapabilityConditionElement CreateListItem(BehaviourCondition item)
        {
            return new CapabilityConditionElement(this.scriptable, this.generator);
        }

        protected override void BindListItem(CapabilityConditionElement element, BehaviourCondition item, int index)
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