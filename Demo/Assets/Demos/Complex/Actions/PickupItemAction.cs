using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Actions
{
    public class PickupItemAction<THoldable> : ActionBase<PickupItemAction<THoldable>.Data>, IInjectable
        where THoldable : IHoldable
    {
        private ItemCollection itemCollection;

        public void Inject(GoapInjector injector)
        {
            this.itemCollection = injector.itemCollection;
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = 0.5f;
            
            var transformTarget = data.Target as TransformTarget;
            
            if (transformTarget == null)
                return;
            
            var holdable = transformTarget.Transform.GetComponent<THoldable>();

            // If this target is claimed by another agent, find another one
            if (holdable.IsClaimed)
            {
                holdable = this.itemCollection.GetFiltered<THoldable>(false, false, false).FirstOrDefault();
                
                if (holdable != null)
                    data.Target = new TransformTarget(holdable.gameObject.transform);
            }
            
            if (holdable == null)
                return;

            holdable.Claim();
            data.Holdable = holdable;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Holdable is null)
                return ActionRunState.Stop;
         
            data.Timer -= context.DeltaTime;
            
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            data.Inventory.Add(data.Holdable);
            
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            public IHoldable Holdable { get; set; }
            public float Timer { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }
    }
}