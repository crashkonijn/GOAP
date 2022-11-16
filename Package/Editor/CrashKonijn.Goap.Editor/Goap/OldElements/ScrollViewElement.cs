using System;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.OldElements
{
    public class ScrollViewElement : IEditorElement
    {
        private readonly IEditorElement child;

        public ScrollViewElement(IEditorElement child)
        {
            this.child = child;
        }

        public VisualElement Render(EditorState state, Action onChange)
        {
            var view = new ScrollView(ScrollViewMode.VerticalAndHorizontal);

            view.Add(this.child.Render(state, onChange));
            
            return view;
        }
    }
}