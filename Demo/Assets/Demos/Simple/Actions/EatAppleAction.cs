using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;
using Demos.Shared.Behaviours;
using Demos.Simple.Behaviours;
using UnityEngine;

namespace Demos.Simple.Actions
{
    public class EatAppleAction : ActionBase<EatAppleAction.Data>, IInjectable
    {
        private InstanceHandler instanceHandler;

        public void Inject(GoapInjector injector)
        {
            this.instanceHandler = injector.instanceHandler;
        }

        public override void OnStart(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget)
                return;

            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return;
            
            data.Apple =  inventory.Get();
            data.Hunger = agent.GetComponent<HungerBehaviour>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Apple == null || data.Hunger == null)
                return ActionRunState.Stop;

            var eatNutrition = context.DeltaTime * 20f;

            data.Apple.nutritionValue -= eatNutrition;
            data.Hunger.hunger -= eatNutrition;
            
            if (data.Apple.nutritionValue <= 0)
                this.instanceHandler.Destroy(data.Apple as IHoldable);
            
            return ActionRunState.Continue;
        }
        
        public override void OnEnd(IMonoAgent agent, Data data)
        {
            if (data.Apple == null)
                return;
            
            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return;
            
            inventory.Put(data.Apple);
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public AppleBehaviour Apple { get; set; }
            public HungerBehaviour Hunger { get; set; }
        }
    }
}