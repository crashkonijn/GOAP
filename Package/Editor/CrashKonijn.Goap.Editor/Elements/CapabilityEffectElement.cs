﻿using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class CapabilityEffectElement : VisualElement, IFoldable
    {
        public Foldout Foldout { get; set; }
        public ClassRefField WorldKeyField { get; set; }

        public EnumField DirectionField { get; set; }
        
        public CapabilityEffectElement(CapabilityConfigScriptable scriptable, GeneratorScriptable generator)
        {
            this.Foldout = new Foldout
            {
                value = false,
            };
            this.Add(this.Foldout);

            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            
            this.WorldKeyField = new ClassRefField();
            this.WorldKeyField.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
            row.Add(this.WorldKeyField);

            this.DirectionField = new EnumField(EffectType.Increase);
            this.DirectionField.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
            row.Add(this.DirectionField);
            
            this.Foldout.Add(row);
        }
    }
}