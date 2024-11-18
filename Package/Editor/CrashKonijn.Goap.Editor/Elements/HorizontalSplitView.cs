using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class HorizontalSplitView : VisualElement
    {
        private VisualElement leftElement;
        private VisualElement rightElement;
        private float leftWidthPercentage;

        public HorizontalSplitView(VisualElement left, VisualElement right, float percentage)
        {
            // Ensure the percentage is between 0 and 100
            this.leftWidthPercentage = Mathf.Clamp(percentage, 0f, 100f);

            this.leftElement = left;
            this.rightElement = right;

            // Style the HorizontalSplitView
            this.style.flexDirection = FlexDirection.Row;

            // Add and style children
            this.Add(this.leftElement);
            this.Add(this.rightElement);

            this.UpdateLayout();
        }

        private void UpdateLayout()
        {
            this.leftElement.style.flexGrow = 0;
            this.leftElement.style.flexBasis = Length.Percent(this.leftWidthPercentage);

            this.rightElement.style.flexGrow = 1;
            this.rightElement.style.flexBasis = Length.Percent(100 - this.leftWidthPercentage);
        }

        public void SetLeftWidthPercentage(float percentage)
        {
            this.leftWidthPercentage = Mathf.Clamp(percentage, 0f, 100f);
            this.UpdateLayout();
        }

        public void ReplaceLeftElement(VisualElement newLeftElement)
        {
            this.leftElement = newLeftElement;
            this.Clear();
            this.Add(this.leftElement);
            this.Add(this.rightElement);
            this.UpdateLayout();
        }
    }
}