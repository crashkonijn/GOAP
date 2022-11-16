using System;
using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap
{
    public class GraphResolver : IGraphResolver
    {
        private readonly IConditionObserver conditionObserver;
        private readonly ICostObserver costObserver;
        private readonly IActionKeyResolver keyResolver;
        private readonly AStar aStar;
        
        internal readonly Graph Graph;

        public GraphResolver(
            HashSet<IAction> actions, 
            IConditionObserver conditionObserver, 
            ICostObserver costObserver, 
            IActionKeyResolver keyResolver)
        {
            this.conditionObserver = conditionObserver;
            this.costObserver = costObserver;
            this.keyResolver = keyResolver;
            this.Graph = new GraphBuilder(keyResolver).Build(actions);
            this.aStar = new AStar(conditionObserver, costObserver);
        }

        public (IAction Action, List<IAction> Path) Resolve(IAction rootAction)
        {
            var rootNode = Graph.RootNodes.FirstOrDefault(x => x.Action == rootAction);
            
            if (rootNode == null)
                throw new Exception($"Root action ({nameof(rootAction)}) does not exist in graph");

            var result = aStar.Find(rootNode);
            if (result == null)
                throw new Exception("Impossibru");
            
            return (result.Value.Node.Action, result.Value.Path.Select(x => x.Action).ToList());
        }
    }
}