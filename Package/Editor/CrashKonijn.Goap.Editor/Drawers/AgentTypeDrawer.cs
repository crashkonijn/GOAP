using CrashKonijn.Goap.Core;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class AgentTypeDrawer : VisualElement
    {
        public  AgentTypeDrawer(IAgentType agentType)
        {
            this.name = "agent-type";
            
            var card = new Card((card) =>
            {
                card.Add(new Header(agentType.Id));

                var root = new VisualElement();

                card.schedule.Execute(() =>
                {
                    root.Clear();
                    root.Add(new Label($"Count: {agentType.Agents.All().Count}"));
                }).Every(1000);
                
                card.Add(root);
            });
            
            this.Add(card);
        }
    }
}