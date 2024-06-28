using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class Circle : VisualElement
    {
        public Circle(Color color, float size)
        {
            // Set the size of the circle
            this.style.width = size;
            this.style.height = size;
            
            this.style.maxWidth = size;
            this.style.maxHeight = size;

            // Set border radius to half of the size to make it a circle
            this.style.borderBottomLeftRadius = size / 2;
            this.style.borderBottomRightRadius = size / 2;
            this.style.borderTopLeftRadius = size / 2;
            this.style.borderTopRightRadius = size / 2;
            
            this.SetColor(color);
        }

        public void SetColor(Color color)
        {
            // Set the background color
            this.style.backgroundColor = color; // Replace with your desired color
        }
    }
}