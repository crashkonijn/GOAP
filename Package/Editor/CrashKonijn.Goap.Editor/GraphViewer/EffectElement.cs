using CrashKonijn.Goap.Core;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class EffectElement : VisualElement
    {
        public INodeEffect GraphEffect { get; }

        public EffectElement(INodeEffect graphEffect)
        {
            this.GraphEffect = graphEffect;
            this.AddToClassList("effect");
            
            this.Label = new Label(this.GetText(graphEffect.Effect));
            this.Add(this.Label);
        }
        
        private string GetText(IEffect effect)
        {
            var suffix = effect.Increase ? "++" : "--";

            return $"{effect.WorldKey.Name}{suffix}";
        }

        public Label Label { get; set; }
    }
}