using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Attributes.Ids;
using UnityEngine;

namespace CrashKonijn.Goap.GenTest
{
    [ActionId("PluckApple-918c6789-79ee-4198-a6d8-dab401447fd7")]
    public class PluckAppleAction : ActionBase<PluckAppleAction.Data>
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