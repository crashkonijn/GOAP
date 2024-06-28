using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class GraphBuilder
    {
        private readonly IKeyResolver keyResolver;

        public GraphBuilder(IKeyResolver keyResolver)
        {
            this.keyResolver = keyResolver;
        }

        public Graph Build(IEnumerable<IConnectable> actions)
        {
            var nodes = actions.ToNodes();

            var graph = new Graph
            {
                RootNodes = nodes.RootNodes.ToList(),
            };

            var allNodes = nodes.RootNodes.Union(nodes.ChildNodes).ToArray();

            var effectMap = this.GetEffectMap(allNodes);
            var conditionMap = this.GetConditionMap(allNodes);

            foreach (var node in nodes.RootNodes)
            {
                this.ConnectNodes(node, effectMap, conditionMap, graph);
            }

            graph.UnconnectedNodes = allNodes.Where(x => !graph.ChildNodes.Contains(x) && !graph.RootNodes.Contains(x))
                .ToArray();

            return graph;
        }

        private void ConnectNodes(INode node, Dictionary<string, List<INode>> effectMap,
            Dictionary<string, List<INode>> conditionMap, IGraph graph)
        {
            if (!graph.ChildNodes.Contains(node) && !node.IsRootNode)
                graph.ChildNodes.Add(node);

            foreach (var actionNodeCondition in node.Conditions)
            {
                if (actionNodeCondition.Connections.Any())
                    continue;

                var key = this.keyResolver.GetKey(node.Action, actionNodeCondition.Condition);

                if (!effectMap.ContainsKey(key))
                    continue;

                actionNodeCondition.Connections = effectMap[key].ToArray();

                foreach (var connection in actionNodeCondition.Connections)
                {
                    connection.Effects.First(x => this.keyResolver.GetKey(connection.Action, x.Effect) == key)
                        .Connections = conditionMap[key].ToArray();
                }

                foreach (var subNode in actionNodeCondition.Connections)
                {
                    this.ConnectNodes(subNode, effectMap, conditionMap, graph);
                }
            }
        }

        private Dictionary<string, List<INode>> GetEffectMap(INode[] actionNodes)
        {
            var map = new Dictionary<string, List<INode>>();

            foreach (var actionNode in actionNodes)
            {
                foreach (var actionNodeEffect in actionNode.Effects)
                {
                    var key = this.keyResolver.GetKey(actionNode.Action, actionNodeEffect.Effect);

                    if (!map.ContainsKey(key))
                        map[key] = new List<INode>();

                    map[key].Add(actionNode);
                }
            }

            return map;
        }

        private Dictionary<string, List<INode>> GetConditionMap(INode[] actionNodes)
        {
            var map = new Dictionary<string, List<INode>>();

            foreach (var actionNode in actionNodes)
            {
                foreach (var actionNodeConditions in actionNode.Conditions)
                {
                    var key = this.keyResolver.GetKey(actionNode.Action, actionNodeConditions.Condition);

                    if (!map.ContainsKey(key))
                        map[key] = new List<INode>();

                    map[key].Add(actionNode);
                }
            }

            return map;
        }
    }
}