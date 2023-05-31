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
                card.schedule.Execute(() =>
                {
                    card.Clear();
                    card.Add(new Header("Agent Data"));
                    card.Add(new ObjectDrawer(agent.CurrentActionData));

                    if (!string.IsNullOrEmpty(agent.NodeViewerFreeformDebugOutput))
                    {
                        card.Add(new Label("\n<b>Freeform Debug Data:</b>"));
                        card.Add(new Label(agent.NodeViewerFreeformDebugOutput));
                    }
                }).Every(500);
            });
            
            this.Add(card);
        }
    }
}