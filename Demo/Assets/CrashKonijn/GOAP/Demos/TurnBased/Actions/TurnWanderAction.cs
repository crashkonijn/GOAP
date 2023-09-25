using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.TurnBased.Actions
{
    public class TurnWanderAction : ActionBase<TurnWanderAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }

        public override void Created()
        {
            
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            Debug.Log($"Performing wander action for {agent}");
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}