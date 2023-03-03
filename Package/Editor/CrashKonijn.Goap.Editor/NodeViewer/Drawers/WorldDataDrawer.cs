using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class WorldDataDrawer : VisualElement
    {
        public WorldDataDrawer(IWorldData worldData)
        {
            this.name = "world-data";
            
            var card = new Card((card) =>
            {
                card.Add(new Header("Conditions"));
                
                foreach (var worldKey in worldData.States)
                {
                    card.Add(new Label(worldKey.Name));
                }
            });
            
            this.Add(card);
        }
    }
}