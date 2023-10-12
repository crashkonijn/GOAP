using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Attributes.Ids;
using UnityEngine;

namespace CrashKonijn.Goap.GenTest
{
    [ActionId("PickupApple-6cb0c207-aaaf-4b92-949e-04832249cc4b")]
    public class PickupAppleAction : ActionBase<PickupAppleAction.Data>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }
    }
}