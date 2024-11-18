using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public static class Extensions
    {
        public static T Add<T>(this VisualElement parent, T child, Action<T> callback) where T : VisualElement
        {
            parent.Add(child);

            callback?.Invoke(child);
            
            return child;
        }

        public static float GetCost(this INode node, IGoapActionProvider provider)
        {
            var agent = provider.Receiver;
            
            if (node.Action is IGoapAction action)
            {
                return action.GetCost(agent, agent.Injector, provider.WorldData.GetTarget(action));
            }
            
            if (node.Action is IGoal goal)
            {
                return goal.GetCost(agent, agent.Injector);
            }
            
            return 0;
        }
    }
}