using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    [CustomEditor(typeof(GoapBehaviour))]
    public class GoapRunnerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var runner = (GoapBehaviour) this.target;
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

        private void RenderConfigFactories(VisualElement root, GoapBehaviour runner)
        {
            root.Add(new PropertyField(this.serializedObject.FindProperty("agentTypeConfigFactories")));
        }
    }
}