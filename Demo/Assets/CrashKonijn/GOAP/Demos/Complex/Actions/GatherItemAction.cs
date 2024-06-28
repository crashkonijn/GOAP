using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Goap;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class GatherItemAction<TGatherable> : GoapActionBase<GatherItemAction<TGatherable>.Data, GatherItemAction<TGatherable>.Props>, IInjectable
        where TGatherable : ItemBase
    {
        private ItemFactory itemFactory;

        public void Inject(GoapInjector injector)
        {
            this.itemFactory = injector.itemFactory;
        }

        public override void Created()
        {
        }
        
        public override void Start(IMonoAgent agent, Data data)
        {
            // There is a normal and slow version of this action
            // based on whether  the agent is holding a (pick)axe
            data.WaitState = ActionRunState.Wait(this.Properties.timer);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.WaitState.IsRunning())
                return data.WaitState;
            
            var item = this.itemFactory.Instantiate<TGatherable>();
            
            item.transform.position = this.GetRandomPosition(agent);
            
            if (this.Properties.pickupItem)
                data.Inventory.Add(item);
            
            return ActionRunState.Stop;
        }

        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return agent.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
        
        [Serializable]
        public class Props : IActionProperties
        {
            public bool pickupItem = false;
            public float timer = 3f;
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IActionRunState WaitState { get; set; }
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }
    }
}