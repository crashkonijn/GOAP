using System.Linq;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class NodeDrawer : Box
    {
        public NodeDrawer(RenderNode node, IAgent agent)
        {
            this.Clear();
            
            this.name = "node-viewer__node";

            this.style.width = node.Rect.width;
            // this.style.height = node.Rect.height;

            this.transform.position = node.Position;
            this.Add(new Label(node.Node.Action.GetType().GetGenericTypeName())
            {
                name = "node-viewer__node__label"
            });

            var root = new VisualElement();
            
            this.Add(root);

            this.schedule.Execute(() =>
            {
                this.ClearClassList();
                this.AddToClassList(this.GetClass(node, agent));
                
                root.Clear();
                this.RenderAction(root, agent, node.Node.Action as IActionBase);
                this.RenderGoal(root, agent, node.Node.Action as IGoalBase);
            }).Every(500);
        }

        private string GetClass(RenderNode node, IAgent agent)
        {
            if (agent.CurrentGoal == node.Node.Action)
                return "node-viewer__node--path";
            
            if (agent.CurrentAction == node.Node.Action)
                return "node-viewer__node--active";
            
            if (agent.CurrentActionPath.Contains(node.Node.Action))
                return "node-viewer__node--path";
            
            return "node-viewer__node--normal";
        }

        private void RenderGoal(VisualElement root, IAgent agent, IGoalBase goal)
        {
            if (goal == null)
                return;

            var conditionObserver = agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            var conditions = goal.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));

            var text = $"<b>Conditions:</b>\n{string.Join("\n", conditions)}";
            
            root.Add(new Label(text));
        }

        private void RenderAction(VisualElement root, IAgent agent, IActionBase action)
        {
            if (action == null)
                return;

            if (agent.WorldData == null)
                return;

            var conditionObserver = agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            var conditions = action.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));
            var effects = action.Effects.Select(x => this.GetText(x as IEffect));

            var cost = action.GetCost(agent as IMonoAgent, agent.Injector);
            
            var target = agent.WorldData.GetTarget(action);

            var text = $"<b>Cost:</b> {cost}\n<b>Target:</b>\n";
            
            if (target != null)
                text += $"    {action.Config.Target.Name}\n    {target.Position}\n";

            text += $"\n<b>Effects</b>:\n{string.Join("\n", effects)}\n<b>Conditions</b>:\n{string.Join("\n", conditions)}";
            
            root.Add(new Label(text));
        }
        
        private string GetText(ICondition condition, bool value)
        {
            var color = value ? "green" : "red";

            return $"    <color={color}>{condition.WorldKey.Name} {GetText(condition.Comparison)} {condition.Amount}</color>";
        }

        private string GetText(IEffect effect)
        {
            var suffix = effect.Increase ? "++" : "--";

            return $"    {effect.WorldKey.Name}{suffix}";
        }

        private string GetText(Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.GreaterThan:
                    return ">";
                case Comparison.GreaterThanOrEqual:
                    return ">=";
                case Comparison.SmallerThan:
                    return "<";
                case Comparison.SmallerThanOrEqual:
                    return "<=";
            }
            
            return "";
        }
    }
}