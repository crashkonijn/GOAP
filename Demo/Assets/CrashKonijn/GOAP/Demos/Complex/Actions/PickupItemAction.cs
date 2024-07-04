using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Goap;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class PickupItemAction<THoldable> : GoapActionBase<PickupItemAction<THoldable>.Data>, IInjectable
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
            data.Timer = ActionRunState.Wait(0.5f);
            
            var transformTarget = data.Target as TransformTarget;
            
            if (transformTarget == null)
                return;
            
            var holdable = transformTarget.Transform.GetComponent<THoldable>();

            // If this target is claimed by another agent, find another one
            if (holdable.IsClaimed && holdable.IsClaimedBy != agent.gameObject)
            {
                holdable = this.itemCollection.GetFiltered<THoldable>(false, false, agent.gameObject).FirstOrDefault();
                
                if (holdable != null)
                    data.Target = new TransformTarget(holdable.gameObject.transform);
            }
            
            if (holdable == null)
                return;

            holdable.Claim(agent.gameObject);
            data.Holdable = holdable;
        }

        public override bool IsValid(IActionReceiver agent, Data data)
        {
            if (data.Holdable == null)
                return false;
            
            return true;
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Timer.IsRunning())
                return data.Timer;
            
            data.Inventory.Add(data.Holdable);
            
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
            
            public IHoldable Holdable { get; set; }
            public IActionRunState Timer { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }
    }
}