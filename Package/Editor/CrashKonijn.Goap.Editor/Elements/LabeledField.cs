using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class LabeledField<T> : VisualElement
        where T : VisualElement, new()
    {
        public Label Label { get; private set; }
        public T Field { get; private set; }

        public LabeledField(string labelText)
        {
            this.Init(labelText, new T());
        }

        public LabeledField(string labelText, T field)
        {
            this.Init(labelText, field);
        }

        private void Init(string labelText, T field)
        {
            this.style.flexDirection = FlexDirection.Row;

            this.Label = new Label(labelText);
            this.Label.style.width = new StyleLength(new Length(40, LengthUnit.Percent));
            this.Add(this.Label);

            this.Field = field;
            this.Field.style.width = new StyleLength(new Length(60, LengthUnit.Percent));
            this.Add(this.Field);
        }
    }
}