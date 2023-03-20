using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Enums;
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
                
                foreach (var (key, state) in worldData.States)
                {
                    card.Add(new Label(this.GetText(key, state)));
                }
            });
            
            this.Add(card);
        }
        
        private string GetText(IWorldKey worldKey, int value)
        {
            return  $"{worldKey.Name} ({value})";
        }
    }
}