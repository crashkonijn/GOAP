using System;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.OldElements
{
    public class SplitViewElement
    {
        private readonly IEditorElement leftElement;
        private readonly IEditorElement rightElement;

        public SplitViewElement(IEditorElement leftElement, IEditorElement rightElement)
        {
            this.leftElement = leftElement;
            this.rightElement = rightElement;
        }
        
        public VisualElement Render(EditorState state, Action onChange)
        {
            // Create a two-pane view with the left pane being fixed with
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            
            splitView.Add(this.leftElement.Render(state, onChange));
            splitView.Add(this.rightElement.Render(state, onChange));
            
            return splitView;
        }
    }
}