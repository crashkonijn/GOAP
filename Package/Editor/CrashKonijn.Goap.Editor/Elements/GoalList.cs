using System.Linq;
using CrashKonijn.Goap.Runtime;
using UnityEditor;

namespace CrashKonijn.Goap.Editor
{
    public class GoalList : ListElementBase<CapabilityGoal, CapabilityGoalElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public GoalList(SerializedObject serializedObject, CapabilityConfigScriptable scriptable, GeneratorScriptable generator) : base(serializedObject.FindProperty("goals"), scriptable.goals)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilityGoalElement CreateListItem(SerializedProperty property, CapabilityGoal item)
        {
            return new CapabilityGoalElement(property, this.scriptable, this.generator, item);
        }

        protected override void BindListItem(SerializedProperty property, CapabilityGoalElement element, CapabilityGoal item, int index)
        {
            element.Foldout.text = item.goal.Name;
            
            element.Goal.Bind(this.scriptable, item.goal, this.generator.GetGoals().ToArray(), classRef =>
            {
                element.Foldout.text = item.goal.Name;
            });
        }
    }
}