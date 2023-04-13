using System.Linq;
using CrashKonijn.Goap.Editor.Classes.Models;

namespace CrashKonijn.Goap.Editor.Classes
{
    public static class Mappers
    {
        public static Graph ToPublic(this Resolver.Models.Graph original)
        {
            return new Graph
            {
                RootNodes = original.RootNodes.ToDictionary(x => x.Guid, x=> x.ToPublic()),
                ChildNodes = original.ChildNodes.ToDictionary(x => x.Guid, x=> x.ToPublic())
            };
        }

        public static Node ToPublic(this Resolver.Models.Node original)
        {
            return new Node
            {
                Action = original.Action,
                Effects = original.Effects.Select(x => x.ToPublic()).ToArray(),
                Conditions = original.Conditions.Select(x => x.ToPublic()).ToArray()
            };
        }

        public static NodeCondition ToPublic(this Resolver.Models.NodeCondition original)
        {
            return new NodeCondition
            {
                Condition = original.Condition.GetType().Name,
                Connections = original.Connections.Select(originalConnection => originalConnection.Action.Guid).ToArray()
            };
        }

        public static NodeEffect ToPublic(this Resolver.Models.NodeEffect original)
        {
            return new NodeEffect
            {
                Effect = original.Effect,
                Connections = original.Connections.Select(originalConnection => originalConnection.Action.Guid).ToArray()
            };
        }
    }
}