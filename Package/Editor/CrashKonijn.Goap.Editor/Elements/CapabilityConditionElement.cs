using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine.UIElements;

#if UNITY_2021
using UnityEditor.UIElements;
#endif

namespace CrashKonijn.Goap.Editor
{
    public class CapabilityConditionElement : VisualElement, IFoldable
    {
        public Foldout Foldout { get; set; }
        public IntegerField AmountField { get; set; }
        public EnumField ComparisonField { get; set; }
        public ClassRefField WorldKeyField { get; set; }
        
        public CapabilityConditionElement(CapabilityConfigScriptable scriptable, GeneratorScriptable generator)
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
            
            this.ComparisonField = new EnumField(Comparison.GreaterThan);
            this.ComparisonField.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            row.Add(this.ComparisonField);

            this.AmountField = new IntegerField();
            this.AmountField.style.width = new StyleLength(new Length(20, LengthUnit.Percent));
            row.Add(this.AmountField);
            
            this.Foldout.Add(row);
        }
    }
}