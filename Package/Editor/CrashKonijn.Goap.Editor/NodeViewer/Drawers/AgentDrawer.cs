using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class AgentDrawer : Box
    {
        public AgentDrawer()
        {
            this.name = "agent";
        }

        public void Update(AgentBehaviour agent)
        {
            this.Clear();

            if (agent == null)
                return;
            
            this.Add(new Label(agent.name));
            this.Add(new Label("Goal: " + agent.CurrentGoal?.GetType().GetGenericTypeName()));
            this.Add(new Label("Action: " + agent.CurrentAction?.GetType().GetGenericTypeName()));
            this.Add(new Label("State: " + agent.State));
        }
    }
}