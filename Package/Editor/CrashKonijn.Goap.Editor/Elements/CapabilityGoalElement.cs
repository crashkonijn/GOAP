using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Generators;
using CrashKonijn.Goap.Support.Loaders;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class CapabilityGoalElement : VisualElement, IFoldable
    {
        public Foldout Foldout { get; private set; }
        public ClassRefField Goal { get; private set; }
        
        public CapabilityGoalElement(CapabilityConfigScriptable scriptable, GeneratorScriptable generator, BehaviourGoal item)
        {
            this.Foldout = new Foldout
            {
                value = false
            };
            this.Add(this.Foldout);

            var card = new Card((card) =>
            {
                var goal = new LabeledField<ClassRefField>("Goal", new ClassRefField());
                this.Goal = goal.Field;
                card.Add(goal);

                card.Add(new Label("Conditions"));
                var conditions = new ConditionList(scriptable, generator, item.conditions);
                card.Add(conditions);
            });
            
            this.Foldout.Add(card);
        }
    }
}