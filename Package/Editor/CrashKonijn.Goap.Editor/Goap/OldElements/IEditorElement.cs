using System;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.OldElements
{
    public interface IEditorElement
    {
        public VisualElement Render(EditorState state, Action onChange);
    }
}