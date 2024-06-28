using CrashKonijn.Agent;
using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Device;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoapActionProvider))]
    public class GoapActionProviderEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss"));

            root.Add(new PropertyField(this.serializedObject.FindProperty("<AgentTypeBehaviour>k__BackingField")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("<LoggerConfig>k__BackingField")));

            if (!Application.isPlaying)
                return root;
            
            var agent = (GoapActionProvider) this.target;
            
            var currentGoal = agent.CurrentGoal;

            root.Add(new Card((card) =>
            {
                card.Add(new Label(), (label) =>
                {
                    label.text = this.GetText(agent);
                    
                    label.schedule.Execute(() =>
                    {
                        if (currentGoal == agent.CurrentGoal)
                            return;
                        
                        currentGoal = agent.CurrentGoal;
                        label.text = this.GetText(agent);
                    }).Every(33);
                });
            }));
            
            root.Add(new WorldDataDrawer(agent.WorldData));
            
            root.Add(new LogDrawer(agent.Logger));

            return root;
        }

        private string GetText(GoapActionProvider provider)
        {
            return $@"Goal: {Runtime.Extensions.GetGenericTypeName(provider.CurrentGoal?.GetType())}
AgentType: {provider.AgentType.Id}
Receiver: {Runtime.Extensions.GetGenericTypeName(provider.Agent?.GetType())}";
        }
    }
}