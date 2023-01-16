using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Editor.Goap.OldElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.Elements
{
    public class ReferencesElement<T> : IEditorElement
    {
        private readonly string label;
        private readonly List<T> elements;
        private readonly Func<T, ReferenceElement.Data> data;

        public ReferencesElement(string label, List<T> elements, Func<T, ReferenceElement.Data> data)
        {
            this.label = label;
            this.elements = elements;
            this.data = data;
        }
        
        public VisualElement Render(EditorState state, Action onChange)
        {
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor/Goap/Views/References.uxml");
            var root = visualTree.Instantiate();

            root.Q<Label>("label").text = this.label;

            var slot = root.Q("slot");
            
            foreach (var element in this.elements)
            {
                slot.Add(new ReferenceElement(this.data(element)).Render(state, onChange));
            }
            
            return root;
        }
    }
}