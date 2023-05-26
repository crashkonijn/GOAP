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
                root.Add(new Header("Goap-Sets"));
                foreach (var goapSet in runner.GoapSets)
                {
                    root.Add(new GoapSetDrawer(goapSet));
                }
            }
            
            return root;
        }

        private void RenderConfigFactories(VisualElement root, GoapRunnerBehaviour runner)
        {
            // TODO: Remove at a later date
#pragma warning disable CS0618
            if (runner.setConfigFactories.Any())
            {
                var oldDataRoot = new VisualElement();
                
                oldDataRoot.Add(new PropertyField(this.serializedObject.FindProperty("setConfigFactories")));
             
                var button = new Button(() =>
                {
                    runner.goapSetConfigFactories.AddRange(runner.setConfigFactories);
                    runner.setConfigFactories.Clear();
                    EditorUtility.SetDirty(runner);
                    oldDataRoot.Clear();
                });
                button.Add(new Label("Migrate data to goapSetConfigFactories"));

                var helpBox = new HelpBox("", HelpBoxMessageType.Error);
                helpBox.Add(button);
                
                oldDataRoot.Add(helpBox);

                root.Add(oldDataRoot);
            }
#pragma warning restore CS0618
            
            root.Add(new PropertyField(this.serializedObject.FindProperty("goapSetConfigFactories")));
        }
    }
}