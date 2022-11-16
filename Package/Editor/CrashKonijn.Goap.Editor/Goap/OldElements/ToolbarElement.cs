using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.OldElements
{
    public class ToolbarElement
    {
        public VisualElement Render(EditorState state, Action onChange)
        {
            var toolbar = new Toolbar();

            toolbar.Add(this.SetupToolbarButton(state, onChange, GoapEditorWindow.GoapEditorPage.Sets));
            toolbar.Add(this.SetupToolbarButton(state, onChange, GoapEditorWindow.GoapEditorPage.Goals));
            toolbar.Add(this.SetupToolbarButton(state, onChange, GoapEditorWindow.GoapEditorPage.Actions));

            return toolbar;
        }

        private Button SetupToolbarButton(EditorState state, Action onChange, GoapEditorWindow.GoapEditorPage page)
        {
            var button = new Button
            {
                text = page.ToString(),
            };
            button.clicked += () => {
                state.Page = page;
                onChange();
            };

            return button;
        }
    }
}