using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
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
                    
                    foreach (var (key, state) in worldData.GlobalData?.States ?? new Dictionary<Type, int>())
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