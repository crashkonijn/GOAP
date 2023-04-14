using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Simple.Behaviours;

namespace Demos.Simple.Actions
{
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
