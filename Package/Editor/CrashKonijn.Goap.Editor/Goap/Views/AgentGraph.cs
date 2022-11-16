using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.Views
{
    public class AgentGraph : EditorWindow
    {
        [MenuItem("Tools/AgentGraph")]
        public static void ShowExample()
        {
            AgentGraph wnd = GetWindow<AgentGraph>();
            wnd.titleContent = new GUIContent("AgentGraph");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = this.rootVisualElement;
        
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Packages/LamosInteractive.Goap.Unity/Editor/Goap/Views/AgentGraph.uxml");
        }

        private Graph GetGraph()
        {
            return new Graph();
        }
    }
}