using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap
{
    public class AStar
    {
        private readonly IConditionObserver conditionObserver;
        private readonly ICostObserver costObserver;

        public AStar(IConditionObserver conditionObserver, ICostObserver costObserver)
        {
            this.conditionObserver = conditionObserver;
            this.costObserver = costObserver;
        }

        internal (Node Node, List<Node> Path)? Find(Node start)
        {
            var openList = new HashSet<AStarNode> { new AStarNode(start) };
            var closedList = new HashSet<AStarNode>();

            while (openList.Any())
            {
                var leastCost = openList.OrderBy(x => x.Cost).First();
                openList.Remove(leastCost);

                 if (CanExecute(leastCost, conditionObserver))
                     return (leastCost.Node, leastCost.Path.Select(x => x.Node).ToList());

                var adjacentTiles = GetConnectedNodes(leastCost).OrderBy(x => x.Cost);

                foreach (var successor in adjacentTiles)
                {
                    if (CanExecute(successor, conditionObserver))
                        return (successor.Node, successor.Path.Select(x => x.Node).ToList());
                    
                    if (closedList.Any(x => x.Node == successor.Node))
                        continue;

                    var existing = openList.FirstOrDefault(x => x.Node == successor.Node);
                    if (existing != null)
                    {
                        if (existing.Cost > successor.Cost)
                        {
                            openList.RemoveWhere(x => x.Node == successor.Node);
                            openList.Add(successor);
                        }
                        continue;
                    }

                    openList.Add(successor);
                }

                closedList.Add(leastCost);
            }
            
            return null;
        }

        private bool CanExecute(AStarNode node, IConditionObserver observer)
        {
            if (node.Node.Conditions.Any(x => !observer.IsMet(x.Condition)))
                return false;

            return true;
        }

        private HashSet<AStarNode> GetConnectedNodes(AStarNode node)
        {
            var conditions = node.Node.Conditions.SelectMany(x => x.Connections);
            var nodePath = new List<AStarNode>(node.Path) { node };
            var actionPath = nodePath.Select(y => y.Node.Action).ToList();

            return conditions.Select(x => new AStarNode(x, costObserver.GetCost(x.Action, actionPath), nodePath)).ToHashSet();
        }

        private class AStarNode
        {
            public AStarNode(Node node, float cost = 1, List<AStarNode> path = null)
            {
                Node = node;
                Cost = cost;
                Path = path ?? new List<AStarNode>();
            }

            public List<AStarNode> Path { get; }
            public float Cost { get; }
            public Node Node { get; }
        }
    }
}