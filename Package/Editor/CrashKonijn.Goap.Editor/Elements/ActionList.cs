using System;
using System.Linq;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class ActionList : ListElementBase<BehaviourAction, CapabilityActionElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public ActionList(CapabilityConfigScriptable scriptable, GeneratorScriptable generator) : base(scriptable.actions)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilityActionElement CreateListItem(BehaviourAction item)
        {
            return new CapabilityActionElement(this.scriptable, this.generator, item);
        }

        protected override void BindListItem(CapabilityActionElement element, BehaviourAction item, int index)
        {
            element.Foldout.text = item.action.Name;
            
            element.Action.Bind(this.scriptable, item.action, this.generator.GetActions().ToArray(), classRef =>
            {
                element.Foldout.text = item.action.Name;
            });
            element.Target.Bind(this.scriptable, item.target, this.generator.GetTargetKeys().ToArray(), classRef =>
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