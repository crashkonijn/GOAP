using System.Collections.Generic;
using LamosInteractive.Goap.Debug.Models;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Debug
{
    public class GraphDebugger : GraphResolver
    {
        public GraphDebugger(List<IAction> actions, IConditionObserver conditionObserver, ICostObserver costObserver, IActionKeyResolver keyResolver) : base(actions, conditionObserver, costObserver, keyResolver)
        {
        }

        public Graph GetGraph()
        {
            return this.Graph.ToPublic();
        }
    }
}