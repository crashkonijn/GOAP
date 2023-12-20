using System.Linq;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class GoalList : ListElementBase<BehaviourGoal, CapabilityGoalElement>
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public GoalList(CapabilityConfigScriptable scriptable, GeneratorScriptable generator) : base(scriptable.goals)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilityGoalElement CreateListItem(BehaviourGoal item)
        {
            return new CapabilityGoalElement(this.scriptable, this.generator, item);
        }

        protected override void BindListItem(CapabilityGoalElement element, BehaviourGoal item, int index)
        {
            element.Foldout.text = item.goal.Name;
            
            element.Goal.Bind(this.scriptable, item.goal, this.generator.GetGoals().ToArray(), classRef =>
            {
                element.Foldout.text = item.goal.Name;
            });
        }
    }
}