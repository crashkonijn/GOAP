using CrashKonijn.Goap.Editor.NodeViewer.Classes;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer
{
    [CustomEditor(typeof(Node))]
    public class NodeInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            // Add a simple label
            myInspector.Add(new Label("This is a custom inspector"));

            // Return the finished inspector UI
            return myInspector;
        }
    }
}