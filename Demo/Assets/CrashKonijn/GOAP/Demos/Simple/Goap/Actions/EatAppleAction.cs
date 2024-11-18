using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Actions
{
    [GoapId("Simple-EatAppleAction")]
    public class EatAppleAction : GoapActionBase<EatAppleAction.Data>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Apple =  data.Inventory.Hold();
        }
        
        public override bool IsValid(IActionReceiver agent, Data data)
        {
            if (data.Apple == null)
                return false;
            
            if (data.SimpleHunger == null)
                return false;

            return true;
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Apple == null)
                return ActionRunState.StopAndLog("Apple is null.");
            
            if (data.SimpleHunger == null)
                return ActionRunState.StopAndLog("SimpleHunger is null.");

            var eatNutrition = context.DeltaTime * 20f;

            data.Apple.nutritionValue -= eatNutrition;
            data.SimpleHunger.hunger -= eatNutrition;

            if (data.Apple.nutritionValue <= 0)
            {
                return ActionRunState.Completed;
            }
            
            return ActionRunState.Continue;
        }
        
        public override void Stop(IMonoAgent agent, Data data)
        {
            this.Finish(agent, data);
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
            this.Finish(agent, data);
        }

        private void Finish(IMonoAgent agent, Data data)
        {
            if (data.Apple == null)
                return;
            
            if (data.Apple.nutritionValue <= 0)
            {
                data.Inventory.Drop(data.Apple);
                Object.Destroy(data.Apple.gameObject);
                return;
            }
            
            data.Inventory.Put(data.Apple);
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