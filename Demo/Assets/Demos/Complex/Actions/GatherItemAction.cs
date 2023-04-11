using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Actions
{
    public class GatherItemAction<TGatherable> : ActionBase<GatherItemAction<TGatherable>.Data>, IInjectable
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
            // based on whether or not the agent is holding an (pick)axe
            // We use the cost as a timer
            data.Timer = this.Config.BaseCost;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;
            
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            var item = this.itemFactory.Instantiate<TGatherable>();
            item.transform.position = this.GetRandomPosition(agent);
            
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
        
        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return agent.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public float Timer { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }
    }
}