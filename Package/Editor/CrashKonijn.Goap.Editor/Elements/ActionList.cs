﻿using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class ActionList : ListElementBase<BehaviourAction, CapabilityActionElement>
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

        protected override CapabilityActionElement CreateListItem(SerializedProperty property, BehaviourAction item)
        {
            return new CapabilityActionElement(this.serializedObject, property, this.scriptable, this.generator, item);
        }

        protected override void BindListItem(SerializedProperty property, CapabilityActionElement element, BehaviourAction item, int index)
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
            
            element.InRangeField.value = item.inRange;
            element.InRangeField.RegisterValueChangedCallback(evt =>
            {
                item.inRange = evt.newValue;
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