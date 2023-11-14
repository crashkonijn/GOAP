using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Actions
{
    [GoapId("Simple-PickupAppleAction")]
    public class PickupAppleAction : ActionBase<PickupAppleAction.Data>
    {
        public override void Created()
        {
        }
        
        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Target is not TransformTarget transformTarget)
                return ActionRunState.Stop;
            
            if (transformTarget.Transform == null)
                return ActionRunState.Stop;
            
            var apple = transformTarget.Transform.GetComponent<AppleBehaviour>();

            if (apple == null)
                return ActionRunState.Stop;
            
            // Prevent picking up same apple
            if (apple.IsPickedUp)
                return ActionRunState.Stop;
            
            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return ActionRunState.Stop;
            
            inventory.Put(apple);
            
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
