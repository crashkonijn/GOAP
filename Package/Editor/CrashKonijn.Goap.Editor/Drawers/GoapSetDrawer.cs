using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Drawers
{
    public class GoapSetDrawer : VisualElement
    {
        public  GoapSetDrawer(IGoapSet goapSet)
        {
            this.name = "goap-set";
            
            var card = new Card((card) =>
            {
                card.Add(new Header(goapSet.Id));

                var root = new VisualElement();

                card.schedule.Execute(() =>
                {
                    root.Clear();
                    root.Add(new Label($"Count: {goapSet.Agents.All().Count}"));
                }).Every(1000);
                
                card.Add(root);
            });
            
            this.Add(card);
        }
    }
}