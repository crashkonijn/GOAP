using System.Linq;
using DebugModels = LamosInteractive.Goap.Debug.Models;
using OriginalModels = LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap.Debug
{
    internal static class Mappers
    {
        public static DebugModels.Graph ToPublic(this OriginalModels.Graph original)
        {
            return new DebugModels.Graph
            {
                RootNodes = original.RootNodes.ToDictionary(x => x.Guid, x=> x.ToPublic()),
                ChildNodes = original.ChildNodes.ToDictionary(x => x.Guid, x=> x.ToPublic())
            };
        }

        public static DebugModels.Node ToPublic(this OriginalModels.Node original)
        {
            return new DebugModels.Node
            {
                Action = original.Action,
                Effects = original.Effects.Select(x => x.ToPublic()).ToHashSet(),
                Conditions = original.Conditions.Select(x => x.ToPublic()).ToHashSet()
            };
        }

        public static DebugModels.NodeCondition ToPublic(this OriginalModels.NodeCondition original)
        {
            return new DebugModels.NodeCondition
            {
                Condition = original.Condition.GetType().Name,
                Connections = original.Connections.Select(originalConnection => originalConnection.Action.Guid).ToHashSet()
            };
        }

        public static DebugModels.NodeEffect ToPublic(this OriginalModels.NodeEffect original)
        {
            return new DebugModels.NodeEffect
            {
                Effect = original.Effect,
                Connections = original.Connections.Select(originalConnection => originalConnection.Action.Guid).ToHashSet()
            };
        }
    }
}