using System;
using System.Linq;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Device;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
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
            
            var currentGoal = agent.CurrentPlan;

            root.Add(new Card((card) =>
            {
                card.Add(new Label(), (label) =>
                {
                    label.text = this.GetText(agent);
                    
                    label.schedule.Execute(() =>
                    {
                        if (currentGoal == agent.CurrentPlan)
                            return;
                        
                        currentGoal = agent.CurrentPlan;
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
            var requestGoals = provider.GoalRequest?.Goals?.Select(x => x.GetType().GetGenericTypeName());
            
            return $@"Goal: {provider.CurrentPlan?.Goal?.GetType().GetGenericTypeName()}
Request: {string.Join(", ", requestGoals ?? Array.Empty<string>())}
AgentType: {provider.AgentType?.Id}
Receiver: {provider.Receiver?.GetType().GetGenericTypeName()}";
        }
    }
}