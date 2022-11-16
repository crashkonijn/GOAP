using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.Views
{
    public class GoapSet : EditorWindow
    {
        [MenuItem("Window/UI Toolkit/GoapSet")]
        public static void ShowExample()
        {
            GoapSet wnd = GetWindow<GoapSet>();
            wnd.titleContent = new GUIContent("GoapSet");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = this.rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Packages/LamosInteractive.Goap.Unity/Editor/Goap/Views/GoapSet.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Packages/LamosInteractive.Goap.Unity/Editor/Goap/Views/GoapSet.uss");
            VisualElement labelWithStyle = new Label("Hello World! With Style");
            labelWithStyle.styleSheets.Add(styleSheet);
            root.Add(labelWithStyle);
        }
    }
}