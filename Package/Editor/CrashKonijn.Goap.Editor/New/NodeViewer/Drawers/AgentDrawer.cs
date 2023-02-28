using CrashKonijn.Goap.Behaviours;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.New.NodeViewer.Drawers
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
            this.Add(new Label(agent.name));
            this.Add(new Label("Goal: " + agent.CurrentGoal?.GetType().Name));
            this.Add(new Label("Action: " + agent.CurrentAction?.GetType().Name));
            this.Add(new Label("State: " + agent.State));
        }
    }
}