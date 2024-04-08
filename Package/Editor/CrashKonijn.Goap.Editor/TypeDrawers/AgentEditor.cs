using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Editor.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(AgentBehaviour))]
    public class AgentEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss"));
            
            root.Add(new PropertyField(this.serializedObject.FindProperty("goapSetBehaviour")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("<DistanceMultiplier>k__BackingField")));
            
            if (!Application.isPlaying)
                return root;
            
            var agent = (AgentBehaviour) this.target;
            
            var currentGoal = agent.CurrentGoal;
            var currentAction = agent.CurrentAction;
            var state = agent.State;
            var moveState = agent.MoveState;
            
            root.Add(new Card((card) =>
            {
                card.Add(new Label(), (label) =>
                {
                    label.text = "Goal: " + agent.CurrentGoal?.GetType().GetGenericTypeName();
                    
                    label.schedule.Execute(() =>
                    {
                        if (currentGoal == agent.CurrentGoal)
                            return;
                        
                        currentGoal = agent.CurrentGoal;
                        label.text = "Goal: " + agent.CurrentGoal?.GetType().GetGenericTypeName();
                    }).Every(33);
                });
                
                card.Add(new Label(), (label) =>
                {
                    label.text = "Action: " + agent.CurrentAction?.GetType().GetGenericTypeName();
                    
                    label.schedule.Execute(() =>
                    {
                        if (currentAction == agent.CurrentAction)
                            return;
                        
                        currentAction = agent.CurrentAction;
                        label.text = "Action: " + agent.CurrentAction?.GetType().GetGenericTypeName();
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
                        if (moveState == agent.MoveState)
                            return;
                        
                        moveState = agent.MoveState;
                        label.text = "MoveState: " + agent.MoveState;
                    }).Every(33);
                });
            }));
            
            root.Add(new Card((card) =>
            {
                card.Add(new ObjectDrawer(agent.CurrentActionData));
            }));

            return root;
        }
    }
}
