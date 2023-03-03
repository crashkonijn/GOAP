using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Actions
{
    [CreateAssetMenu(menuName = "Goap/Actions/PickupAppleAction")]
    public class PickupAppleAction : ActionBase<PickupAppleAction.Data>
    {
        public override void OnStart(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data)
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
        
        public override void OnEnd(IMonoAgent agent, Data data)
        {
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }
    }
}

