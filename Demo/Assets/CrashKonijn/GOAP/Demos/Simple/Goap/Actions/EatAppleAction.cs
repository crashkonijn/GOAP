using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Actions
{
    [GoapId("Simple-EatAppleAction")]
    public class EatAppleAction : ActionBase<EatAppleAction.Data>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return;
            
            data.Apple =  inventory.Hold();
            data.SimpleHunger = agent.GetComponent<SimpleHungerBehaviour>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Apple == null || data.SimpleHunger == null)
                return ActionRunState.Stop;

            var eatNutrition = context.DeltaTime * 20f;

            data.Apple.nutritionValue -= eatNutrition;
            data.SimpleHunger.hunger -= eatNutrition;

            if (data.Apple.nutritionValue <= 0)
            {
                data.Inventory.Drop(data.Apple);
                Object.Destroy(data.Apple.gameObject);
            }
            
            return ActionRunState.Continue;
        }
        
        public override void End(IMonoAgent agent, Data data)
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
            
            [GetComponent]
            public SimpleHungerBehaviour SimpleHunger { get; set; }
            
            [GetComponent]
            public InventoryBehaviour Inventory { get; set; }
        }
    }
}