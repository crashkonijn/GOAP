using CrashKonijn.Goap.Behaviours;
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
            
            root.Add(new PropertyField(this.serializedObject.FindProperty("goapSet")));
            
            if (!Application.isPlaying)
                return root;
            
            var agent = (AgentBehaviour) this.target;
            
            var field = new ObjectField("Agent");
            field.Add(new Label("Goal"));
            field.Add(new Label(agent.CurrentGoal?.GetType().Name));
            
            root.Add(field);
            
            root.Add(new Label("Goal: " + agent.CurrentGoal?.GetType().Name));
            root.Add(new Label("Action: " + agent.CurrentAction?.GetType().Name));

            return root;
        }
    }
}