using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.New.NodeViewer.Drawers
{
    public class NodeDrawer : Box
    {
        public NodeDrawer(RenderNode node, IAgent agent)
        {
            this.Clear();
            
            this.name = "node-viewer__node";
            
            this.AddToClassList(this.GetClass(node, agent));

            this.style.width = node.Rect.width;
            this.style.height = node.Rect.height;

            this.transform.position = node.Position;
            this.Add(new Label(node.Node.Action.GetType().Name));
            
            this.RenderAction(agent, node.Node.Action as IActionBase);
            this.RenderGoal(agent, node.Node.Action as IGoalBase);
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

        private void RenderGoal(IAgent agent, IGoalBase goal)
        {
            if (goal == null)
                return;

            var conditionObserver = agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            var conditions = goal.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));

            var text = $"Conditions:\n{string.Join("\n", conditions)}\n";
            
            this.Add(new Label(text));
        }

        private void RenderAction(IAgent agent, IActionBase action)
        {
            if (action == null)
                return;

            var conditionObserver = agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            var conditions = action.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));
            var effects = action.Effects.Select(x => this.GetText(x as IEffect, conditionObserver.IsMet(x)));

            var text = $"Target: {agent.WorldData.GetTarget(action).Position}\n\nEffects:\n{string.Join("\n", effects)}\nConditions:\n{string.Join("\n", conditions)}\n";
            
            this.Add(new Label(text));
        }
        
        private string GetText(ICondition condition, bool value)
        {
            var suffix = condition.Positive ? "true" : "false";
            var color = value ? "green" : "red";

            return $"    <color={color}>{condition.WorldKey.Name} ({suffix})</color>";
        }

        private string GetText(IEffect effect, bool value)
        {
            var suffix = effect.Positive ? "true" : "false";
            var color = value ? "green" : "red";

            return $"    <color={color}>{effect.WorldKey.Name} ({suffix})</color>";
        }
    }
}