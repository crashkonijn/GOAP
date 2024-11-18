using CrashKonijn.Agent.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    [CustomEditor(typeof(AgentBehaviour))]
    public class AgentEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss"));
            
            root.Add(new PropertyField(this.serializedObject.FindProperty("<ActionProviderBase>k__BackingField")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("<DistanceMultiplier>k__BackingField")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("<LoggerConfig>k__BackingField")));
            
            if (!Application.isPlaying)
                return root;
            
            var agent = (AgentBehaviour) this.target;
            
            var currentAction = agent.ActionState.Action;
            var state = agent.State;
            
            root.Add(new Card((card) =>
            {
                card.Add(new Label(), (label) =>
                {
                    label.text = "Action: " + agent.ActionState.Action?.GetType().GetGenericTypeName();
                    
                    label.schedule.Execute(() =>
                    {
                        if (currentAction == agent.ActionState.Action)
                            return;
                        
                        currentAction = agent.ActionState.Action;
                        label.text = "Action: " + agent.ActionState.Action?.GetType().GetGenericTypeName();
                    }).Every(33);
                });
                
                card.Add(new Label(), (label) =>
                {
                    label.text = "State: " + agent.State;
                    
                    label.schedule.Execute(() =>
                    {
                        if (state == agent.State)
                            return;
                        
                        state = agent.State;
                        label.text = "State: " + agent.State;
                    }).Every(33);
                });
                
                card.Add(new Label(), (label) =>
                {
                    label.text = "MoveState: " + agent.MoveState;
                    
                    label.schedule.Execute(() =>
                    {
                        label.text = $"MoveState: {agent.MoveState}\n   Distance: {agent.DistanceObserver.GetDistance(agent, agent.CurrentTarget, agent.Injector):0.00}\n   Stopping distance: {agent.ActionState.Action?.GetStoppingDistance():0.00}";
                    }).Every(33);
                });
            }));
            
            root.Add(new Card((card) =>
            {
                card.Add(new ObjectDrawer(agent.ActionState.Data));
            }));
            
            root.Add(new LogDrawer(agent.Logger));

            return root;
        }
    }
}
