using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class AgentDataDrawer : VisualElement
    {
        public AgentDataDrawer(IAgent agent, IAgentDebugger debugger)
        {
            this.name = "agent-data";
            
            var card = new Card((card) =>
            {
                card.schedule.Execute(() =>
                {
                    card.Clear();
                    card.Add(new Header("Agent Data"));
                    card.Add(new Label(this.GetText(agent, debugger)));
                }).Every(500);
            });
            
            this.Add(card);
        }

        private string GetText(IAgent agent, IAgentDebugger debugger)
        {
            if (debugger == null)
                return "Define an agent debugger\nin the GoapSetConfig to \ncustomize this view.";
            
            if (agent == null)
                return "";
            
            return debugger.GetInfo(agent as IMonoAgent, agent.Injector);
        }
    }
}