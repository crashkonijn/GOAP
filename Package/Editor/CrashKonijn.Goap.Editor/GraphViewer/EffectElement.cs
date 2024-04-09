using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
    public class EffectElement : VisualElement
    {
        private readonly SelectedObject selectedObject;
        public INodeEffect GraphEffect { get; }

        public EffectElement(INodeEffect graphEffect, SelectedObject selectedObject)
        {
            this.selectedObject = selectedObject;
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