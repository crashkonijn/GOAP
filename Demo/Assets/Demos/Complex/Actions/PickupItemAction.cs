using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Actions
{
    public class PickupItemAction<THoldable> : ActionBase<PickupItemAction<THoldable>.Data>
        where THoldable : IHoldable
    {

        public override void OnStart(IMonoAgent agent, Data data)
        {
            var transformTarget = data.Target as TransformTarget;
            
            if (transformTarget == null)
                return;
            
            data.Holdable = transformTarget.Transform.GetComponent<THoldable>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            data.Inventory.Add(data.Holdable);
            
            return ActionRunState.Stop;
        }

        public override void OnEnd(IMonoAgent agent, Data data)
        {
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            public IHoldable Holdable { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }
    }
}