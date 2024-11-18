using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class Header : VisualElement
    {
        public Header(string text)
        {
            this.name = "header";
            this.Add(new Label(text));
        }
    }
}