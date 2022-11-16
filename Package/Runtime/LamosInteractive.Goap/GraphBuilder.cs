﻿using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap
{
    internal class GraphBuilder
    {
        private readonly IActionKeyResolver keyResolver;

        public GraphBuilder(IActionKeyResolver keyResolver)
        {
            this.keyResolver = keyResolver;
        }
        
        public Graph Build(IEnumerable<IAction> actions)
        {
            var nodes = actions.ToNodes();
            
            var graph = new Graph
            {
                RootNodes = nodes.RootNodes,
            };

            var allNodes = nodes.RootNodes.Union(nodes.ChildNodes).ToHashSet();
            
            var effectMap = this.GetEffectMap(allNodes);
            var conditionMap = this.GetConditionMap(allNodes);
            
            foreach (var node in nodes.RootNodes)
            {
                this.ConnectNodes(node, effectMap, conditionMap, graph);
            }

            return graph;
        }

        private void ConnectNodes(Node node, Dictionary<string, HashSet<Node>> effectMap, Dictionary<string, HashSet<Node>> conditionMap, Graph graph)
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
                
                actionNodeCondition.Connections = effectMap[key];
                
                foreach (var connection in actionNodeCondition.Connections)
                {
                    connection.Effects.First(x => this.keyResolver.GetKey(connection.Action, x.Effect) == key).Connections = conditionMap[key];
                }

                foreach (var subNode in actionNodeCondition.Connections)
                {
                    this.ConnectNodes(subNode, effectMap, conditionMap, graph);
                }
            }
        }

        private Dictionary<string, HashSet<Node>> GetEffectMap(HashSet<Node> actionNodes)
        {
            var map = new Dictionary<string, HashSet<Node>>();
            
            foreach (var actionNode in actionNodes)
            {
                foreach (var actionNodeEffect in actionNode.Effects)
                {
                    var key = this.keyResolver.GetKey(actionNode.Action, actionNodeEffect.Effect);
                    
                    if (!map.ContainsKey(key))
                        map[key] = new HashSet<Node>();
                    
                    map[key].Add(actionNode);
                }
            }

            return map;
        }

        private Dictionary<string, HashSet<Node>> GetConditionMap(HashSet<Node> actionNodes)
        {
            var map = new Dictionary<string, HashSet<Node>>();
            
            foreach (var actionNode in actionNodes)
            {
                foreach (var actionNodeConditions in actionNode.Conditions)
                {
                    var key = this.keyResolver.GetKey(actionNode.Action, actionNodeConditions.Condition);
                    
                    if (!map.ContainsKey(key))
                        map[key] = new HashSet<Node>();
                    
                    map[key].Add(actionNode);
                }
            }

            return map;
        }
    }
}