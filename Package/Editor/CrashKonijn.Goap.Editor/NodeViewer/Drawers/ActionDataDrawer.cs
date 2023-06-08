using CrashKonijn.Goap.Editor.Drawers;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class ActionDataDrawer : VisualElement
    {
        public ActionDataDrawer(IAgent agent)
        {
            this.name = "action-data";
            
            var card = new Card((card) =>
            {
                card.schedule.Execute(() =>
                {
                    card.Clear();
                    card.Add(new Header("Action Data"));
                    card.Add(new ObjectDrawer(agent.CurrentActionData));
                }).Every(500);
            });
            
            this.Add(card);
        }
    }
}