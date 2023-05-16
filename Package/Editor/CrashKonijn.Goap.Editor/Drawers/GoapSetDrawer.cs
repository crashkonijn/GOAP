using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Drawers
{
    public class GoapSetDrawer : VisualElement
    {
        public  GoapSetDrawer(IGoapSet set)
        {
            this.name = "goap-set";
            
            var card = new Card((card) =>
            {
                card.Add(new Header(set.Id));

                var root = new VisualElement();

                card.schedule.Execute(() =>
                {
                    root.Clear();
                    root.Add(new Label($"Count: {set.Agents.All().Count}"));
                }).Every(1000);
                
                card.Add(root);
            });
            
            this.Add(card);
        }
    }
}