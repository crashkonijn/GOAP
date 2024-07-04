using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Editor
{
    public static class Mappers
    {
        public static Graph ToPublic(this IGraph original)
        {
            return new Graph
            {
                RootNodes = original.RootNodes.ToDictionary(x => x.Guid, x=> x.ToPublic()),
                ChildNodes = original.ChildNodes.ToDictionary(x => x.Guid, x=> x.ToPublic())
            };
        }

        public static Node ToPublic(this INode original)
        {
            return new Node
            {
                Action = original.Action,
                Effects = original.Effects.Select(x => x.ToPublic()).ToArray(),
                Conditions = original.Conditions.Select(x => x.ToPublic()).ToArray()
            };
        }

        public static NodeCondition ToPublic(this INodeCondition original)
        {
            return new NodeCondition
            {
                Condition = original.Condition.GetType().Name,
                Connections = original.Connections.Select(originalConnection => originalConnection.Action.Guid).ToArray()
            };
        }

        public static NodeEffect ToPublic(this INodeEffect original)
        {
            return new NodeEffect
            {
                Effect = original.Effect,
                Connections = original.Connections.Select(originalConnection => originalConnection.Action.Guid).ToArray()
            };
        }
    }
}