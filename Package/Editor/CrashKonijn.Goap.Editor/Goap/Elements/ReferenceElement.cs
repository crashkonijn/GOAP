using System;
using CrashKonijn.Goap.Editor.Goap.OldElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.Elements
{
    public class ReferenceElement : IEditorElement
    {
        private readonly Data data;

        public ReferenceElement(Data data)
        {
            this.data = data;
        }
        
        public VisualElement Render(EditorState state, Action onChange)
        {
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Packages/LamosInteractive.Goap.Unity/Editor/Goap/Views/Reference.uxml");
            var root = visualTree.Instantiate();

            root.Q<Label>("label").text = this.data.Label;

            return root;
        }

        public class Data
        {
            public string Label { get; set; }
        }
    }
}