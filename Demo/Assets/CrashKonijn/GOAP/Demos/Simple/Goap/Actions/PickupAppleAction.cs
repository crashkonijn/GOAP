using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Actions
{
    [GoapId("Simple-PickupAppleAction")]
    public class PickupAppleAction : GoapActionBase<PickupAppleAction.Data>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget transformTarget)
                return;
            
            if (transformTarget.Transform == null)
                return;
            
            data.Apple = transformTarget.Transform.GetComponent<AppleBehaviour>();
        }

        public override bool IsValid(IActionReceiver agent, Data data)
        {
            if (!base.IsValid(agent, data))
                return false;
            
            if (data.Apple == null)
            {
                agent.Logger.Warning($"No apple found: {data.Target}");
                return false;
            }

            if (data.Apple.IsPickedUp)
            {
                agent.Logger.Warning($"Apple already picked up: {data.Apple.name}");
                return false;
            }
            
            if (data.Inventory == null)
                return false;

            return true;
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Target is not TransformTarget transformTarget)
                return ActionRunState.Stop;
            
            if (transformTarget.Transform == null)
                return ActionRunState.Stop;
            
            data.Inventory.Put(data.Apple);
            agent.Logger.Log($"Picked up apple: {data.Apple.name}");

            return ActionRunState.Completed;
        }
        
        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public AppleBehaviour Apple { get; set; }
            [GetComponent]
            public InventoryBehaviour Inventory { get; set; }
        }
    }
}
