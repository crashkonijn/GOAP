using CrashKonijn.Agent;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Editor.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Device;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoapAgentBehaviour))]
    public class GoapAgentEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss"));

            root.Add(new PropertyField(this.serializedObject.FindProperty("<AgentTypeBehaviour>k__BackingField")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("<LoggerConfig>k__BackingField")));

            if (!Application.isPlaying)
                return root;
            
            var agent = (GoapAgentBehaviour) this.target;
            
            var currentGoal = agent.CurrentGoal;

            root.Add(new Card((card) =>
            {
                card.Add(new Label(), (label) =>
                {
                    label.text = "Goal: " + currentGoal?.GetType().GetGenericTypeName();
                    
                    label.schedule.Execute(() =>
                    {
                        if (currentGoal == agent.CurrentGoal)
                            return;
                        
                        currentGoal = agent.CurrentGoal;
                        label.text = "Goal: " + currentGoal?.GetType().GetGenericTypeName();
                    }).Every(33);
                });
            }));
            
            root.Add(new WorldDataDrawer(agent.WorldData));
            
            root.Add(new LogDrawer(agent.Logger));

            return root;
        }
    }
}