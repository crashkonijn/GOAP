using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class EffectList : ListElementBase<CapabilityEffect, CapabilityEffectElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public EffectList(SerializedProperty serializedProperty, CapabilityConfigScriptable scriptable, GeneratorScriptable generator, List<CapabilityEffect> effects) : base(serializedProperty.FindPropertyRelative("effects"), effects)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilityEffectElement CreateListItem(SerializedProperty property, CapabilityEffect item)
        {
            return new CapabilityEffectElement(this.scriptable, this.generator);
        }

        protected override void BindListItem(SerializedProperty property, CapabilityEffectElement element, CapabilityEffect item, int index)
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