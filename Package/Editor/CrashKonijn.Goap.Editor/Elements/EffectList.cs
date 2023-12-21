using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class EffectList : ListElementBase<BehaviourEffect, CapabilityEffectElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public EffectList(CapabilityConfigScriptable scriptable, GeneratorScriptable generator, List<BehaviourEffect> conditions) : base(conditions)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilityEffectElement CreateListItem(BehaviourEffect item)
        {
            return new CapabilityEffectElement(this.scriptable, this.generator);
        }

        protected override void BindListItem(CapabilityEffectElement element, BehaviourEffect item, int index)
        {
            element.Foldout.text = item.ToString();
            
            element.WorldKeyField.Bind(this.scriptable, item.worldKey, this.generator.GetWorldKeys().ToArray(), classRef =>
            {
                element.Foldout.text = item.ToString();
            });
            
            element.DirectionField.value = item.effect;
            element.DirectionField.RegisterValueChangedCallback(evt =>
            {
                item.effect = (EffectType) evt.newValue;
                element.Foldout.text = item.ToString();
                EditorUtility.SetDirty(this.scriptable);
            });
        }
    }
}