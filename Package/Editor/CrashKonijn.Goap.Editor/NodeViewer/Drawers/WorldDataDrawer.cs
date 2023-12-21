using System;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.Elements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class WorldDataDrawer : VisualElement
    {
        public WorldDataDrawer(ILocalWorldData worldData)
        {
            this.name = "world-data";
            
            var card = new Card((card) =>
            {
                card.Add(new Header("World Data"));
                
                var root = new VisualElement();
                
                card.schedule.Execute(() =>
                {
                    root.Clear();
                    
                    foreach (var (key, state) in worldData.States)
                    {
                        root.Add(new Label(this.GetText(key, state, "local")));
                    }
                    
                    foreach (var (key, state) in worldData.GlobalData.States)
                    {
                        root.Add(new Label(this.GetText(key, state, "global")));
                    }
                }).Every(500);
                
                card.Add(root);
            });
            
            this.Add(card);
        }
        
        private string GetText(Type worldKey, int value, string scope)
        {
            return  $"{worldKey.GetGenericTypeName()}: {value} ({scope})";
        }
    }
}