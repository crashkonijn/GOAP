using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class AgentDataDrawer : VisualElement
    {
        public AgentDataDrawer(IAgent agent)
        {
            this.name = "agent-data";
            
            var card = new Card((card) =>
            {
                card.Add(new Header("Agent Data"));
                card.Add(new ObjectDrawer(agent.CurrentActionData));
            });
            
            this.Add(card);
        }
    }
}