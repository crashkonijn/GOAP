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
    public class GatherItem<TGatherable> : ActionBase<GatherItem<TGatherable>.Data>, IInjectable
        where TGatherable : ItemBase
    {
        private ItemFactory itemFactory;

        public void Inject(GoapInjector injector)
        {
            this.itemFactory = injector.itemFactory;
        }
        
        public override void OnStart(IMonoAgent agent, Data data)
        {
            data.Timer = 1f;
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

        public override void OnEnd(IMonoAgent agent, Data data)
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