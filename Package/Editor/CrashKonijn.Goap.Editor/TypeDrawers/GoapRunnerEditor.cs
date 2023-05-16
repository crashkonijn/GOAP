using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Editor.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoapRunnerBehaviour))]
    public class GoapRunnerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var runner = (GoapRunnerBehaviour) this.target;
            var root = new VisualElement();
            
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss"));

            InspectorElement.FillDefaultInspector(root, this.serializedObject, this);
            
            if (Application.isPlaying)
            {
                root.Add(new Header("Sets"));
                foreach (var set in runner.Sets)
                {
                    root.Add(new GoapSetDrawer(set));
                }
            }
            
            return root;
        }
    }
}