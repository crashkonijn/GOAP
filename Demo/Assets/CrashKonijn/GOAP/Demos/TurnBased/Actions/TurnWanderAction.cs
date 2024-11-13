using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace TurnBased.Actions
{
    public class TurnWanderAction : GoapActionBase<TurnWanderAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }

        public override void Created() { }

        public override void Start(IMonoAgent agent, Data data) { }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            Debug.Log($"Performing wander action for {agent}");
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data) { }
    }
}
