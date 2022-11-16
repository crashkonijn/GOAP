using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap
{
    internal static class Extensions
    {
        public static (HashSet<Node> RootNodes, HashSet<Node> ChildNodes) ToNodes(this IEnumerable<IAction> actions)
        {
            var mappedNodes =actions.Select(ToNode).ToHashSet();
            
            return (
                mappedNodes.Where(x => x.IsRootNode).ToHashSet(),
                mappedNodes.Where(x => !x.IsRootNode).ToHashSet()
            );
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> list)
        {
            return new HashSet<T>(list);
        }

        private static Node ToNode(IAction action)
        {
            return new Node
            {
                Action = action,
                Conditions = action.Conditions?.Select(y => new NodeCondition
                {
                    Condition = y
                }).ToHashSet() ?? new HashSet<NodeCondition>(),
                Effects = action.Effects?.Select(y => new NodeEffect
                {
                    Effect = y
                }).ToHashSet() ?? new HashSet<NodeEffect>()
            };
        }
    }
}