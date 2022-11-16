using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-100)]
    public class GoapResolver : MonoBehaviour
    {
        private GoapConfig config;
        private HashSet<GoapSet> sets = new();
        private AgentCollection agentCollection;

        private void Awake()
        {
            this.agentCollection = this.GetComponent<AgentCollection>();

            this.config = new GoapConfig(
                this.GetComponent<CostObserverBase<IActionBase>>(),
                this.GetComponent<ConditionObserverBase<Condition, Effect>>(),
                this.GetComponent<ActionKeyResolverBase<IActionBase, IGoalBase>>()
            );
        }

        private void Start()
        {
            foreach (var set in this.sets)
            {
                set.Initialize(this.config);
            }
        }

        public void Register(GoapSet set) => this.sets.Add(set);
        public void Register(Agent agent) => this.agentCollection.Add(agent);
        public void Unregister(Agent agent) => this.agentCollection.Remove(agent);

        private void FixedUpdate()
        {
            foreach (var (set, agents) in this.agentCollection.All())
            {
                set.Run(agents);
            }
        }
    }
}