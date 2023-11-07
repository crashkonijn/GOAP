using System.Linq;
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

            root.Add(new PropertyField(this.serializedObject.FindProperty("configInitializer")));
            
            this.RenderConfigFactories(root, runner);
            
            if (Application.isPlaying)
            {
                root.Add(new Header("Agent Types"));
                foreach (var agentTypes in runner.AgentTypes)
                {
                    root.Add(new AgentTypeDrawer(agentTypes));
                }
            }
            
            return root;
        }

        private void RenderConfigFactories(VisualElement root, GoapRunnerBehaviour runner)
        {
            root.Add(new PropertyField(this.serializedObject.FindProperty("agentTypeConfigFactories")));
        }
    }
}