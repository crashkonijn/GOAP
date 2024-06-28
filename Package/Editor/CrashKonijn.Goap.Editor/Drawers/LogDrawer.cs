using System.Linq;
using CrashKonijn.Agent.Core;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class LogDrawer : VisualElement
    {
        private readonly ILogger logger;
        private Label text;

        public LogDrawer(ILogger logger)
        {
            this.logger = logger;
            var card = new Card((card) =>
            {
                card.Add(new Label("Logs:"));
                
                this.text = new Label();
                card.Add(this.text);
            });
            
            this.Add(card);
            
            this.schedule.Execute(() =>
            {
                this.Update();
            }).Every(33);
        }

        private void Update()
        {
            var logs = this.logger.Logs.ToArray().Reverse();
            
            this.text.text = string.Join("\n", logs);
        }
    }
}