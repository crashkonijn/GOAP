using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;
using Demos.Shared.Behaviours;

namespace Demos.Complex.Actions
{
    public class EatAction : ActionBase<EatAction.Data>, IInjectable
    {
        private InstanceHandler instanceHandler;

        public void Inject(GoapInjector injector)
        {
            this.instanceHandler = injector.instanceHandler;
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Eatable = data.Inventory.Get<IEatable>().FirstOrDefault();
            data.Inventory.Hold(data.Eatable);
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Eatable == null)
                return ActionRunState.Stop;
            
            var eatNutrition = context.DeltaTime * 20f;
            data.Eatable.NutritionValue -= eatNutrition;
            data.Hunger.hunger -= eatNutrition;

            if (data.Hunger.hunger <= 20f)
                return ActionRunState.Stop;

            if (data.Eatable.NutritionValue > 0)
                return ActionRunState.Continue;

            if (data.Eatable == null)
                return ActionRunState.Stop;
            
            data.Inventory.Remove(data.Eatable);
            this.instanceHandler.QueueForDestroy(data.Eatable);
            
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            if (data.Eatable == null)
                return;
            
            if (data.Eatable.NutritionValue > 0)
                data.Inventory.Add(data.Eatable);
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IEatable Eatable { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
            
            [GetComponent]
            public HungerBehaviour Hunger { get; set; }
        }
    }
}