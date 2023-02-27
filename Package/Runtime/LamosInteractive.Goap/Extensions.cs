using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap
{
    internal static class Extensions
    {
        public static (Node[] RootNodes, Node[] ChildNodes) ToNodes(this IEnumerable<IAction> actions)
        {
            var mappedNodes =actions.Select(ToNode).ToArray();
            
            return (
                mappedNodes.Where(x => x.IsRootNode).ToArray(),
                mappedNodes.Where(x => !x.IsRootNode).ToArray()
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
                }).ToList() ?? new List<NodeCondition>(),
                Effects = action.Effects?.Select(y => new NodeEffect
                {
                    Effect = y
                }).ToList() ?? new List<NodeEffect>()
            };
        }
    }
}