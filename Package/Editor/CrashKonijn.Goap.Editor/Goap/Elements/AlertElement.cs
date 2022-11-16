using System;
using CrashKonijn.Goap.Editor.Goap.OldElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.Elements
{
    public class AlertElement : IEditorElement
    {
        public AlertType Type { get; set; } = AlertType.Info;
        public string Header { get; set; }
        public string Text { get; set; }
        
        public VisualElement Render(EditorState state, Action onChange)
        {
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Packages/LamosInteractive.Goap.Unity/Editor/Goap/Views/Alert.uxml");
            var root = visualTree.Instantiate();

            root.Q<VisualElement>("root").EnableInClassList($"alert--{this.Type.ToString().ToLower()}", true);
            root.Q<Label>("header").text = this.Header;
            root.Q<TextElement>("text").text = this.Text;

            return root;
        }

        public enum AlertType
        {
            Danger,
            Info
        }
    }
}