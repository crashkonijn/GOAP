using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class CapabilityGoalElement : VisualElement, IFoldable
    {
        public Foldout Foldout { get; private set; }
        public ClassRefField Goal { get; private set; }
        
        public CapabilityGoalElement(SerializedProperty serializedProperty, CapabilityConfigScriptable scriptable, GeneratorScriptable generator, CapabilityGoal item)
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
                var conditions = new ConditionList(serializedProperty, scriptable, generator, item.conditions);
                card.Add(conditions);
            });
            
            this.Foldout.Add(card);
        }
    }
}