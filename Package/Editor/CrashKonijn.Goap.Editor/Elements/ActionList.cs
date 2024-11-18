using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ActionList : ListElementBase<CapabilityAction, CapabilityActionElement>
    {
        private readonly SerializedObject serializedObject;
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public ActionList(SerializedObject serializedObject, CapabilityConfigScriptable scriptable, GeneratorScriptable generator) : base(serializedObject.FindProperty("actions"), scriptable.actions)
        {
            this.serializedObject = serializedObject;
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilityActionElement CreateListItem(SerializedProperty property, CapabilityAction item)
        {
            return new CapabilityActionElement(this.serializedObject, property, this.scriptable, this.generator, item);
        }

        protected override void BindListItem(SerializedProperty property, CapabilityActionElement element, CapabilityAction item, int index)
        {
            element.Foldout.text = item.action.Name;
            
            element.Action.Bind(this.scriptable, item.action, this.generator.GetActions(), classRef =>
            {
                element.Foldout.text = item.action.Name;
                element.Properties.Bind(property, item, this.generator.GetActions());
            });
            
            element.Properties.Bind(property, item, this.generator.GetActions());
            
            element.Target.Bind(this.scriptable, item.target, this.generator.GetTargetKeys(), classRef =>
            {
                element.Foldout.text = item.action.Name;
            });
            
            element.BaseCostField.value = item.baseCost;
            element.BaseCostField.RegisterValueChangedCallback(evt =>
            {
                item.baseCost = evt.newValue;
                EditorUtility.SetDirty(this.scriptable);
            });
            
            element.InRangeField.value = item.stoppingDistance;
            element.InRangeField.RegisterValueChangedCallback(evt =>
            {
                item.stoppingDistance = evt.newValue;
                EditorUtility.SetDirty(this.scriptable);
            });
            
            element.RequiresTargetField.value = item.requiresTarget;
            element.RequiresTargetField.RegisterValueChangedCallback(evt =>
            {
                item.requiresTarget = evt.newValue;
                EditorUtility.SetDirty(this.scriptable);
            });
            
            element.ValidateConditionsField.value = item.validateConditions;
            element.ValidateConditionsField.RegisterValueChangedCallback(evt =>
            {
                item.validateConditions = evt.newValue;
                EditorUtility.SetDirty(this.scriptable);
            });
            
            element.ValidateTargetField.value = item.validateTarget;
            element.ValidateTargetField.RegisterValueChangedCallback(evt =>
            {
                item.validateTarget = evt.newValue;
                EditorUtility.SetDirty(this.scriptable);
            });
            
            element.MoveModeField.value = item.moveMode;
            element.MoveModeField.RegisterValueChangedCallback(evt =>
            {
                item.moveMode = (ActionMoveMode) evt.newValue;
                EditorUtility.SetDirty(this.scriptable);
            });
        }
    }
}