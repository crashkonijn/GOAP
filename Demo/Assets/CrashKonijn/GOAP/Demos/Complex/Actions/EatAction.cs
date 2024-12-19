using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Goap;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class EatAction : GoapActionBase<EatAction.Data>, IInjectable
    {
        private InstanceHandler instanceHandler;

        public void Inject(GoapInjector injector)
        {
            this.instanceHandler = injector.instanceHandler;
        }

        public override void Created() { }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Eatable = data.Inventory.Get<IEatable>().FirstOrDefault();
            data.Inventory.Hold(data.Eatable);
        }

        public override bool IsValid(IActionReceiver agent, Data data)
        {
            if (data.Eatable == null)
                return false;

            return true;
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            var eatNutrition = context.DeltaTime * 20f;
            data.Eatable.NutritionValue -= eatNutrition;
            data.ComplexHunger.hunger -= eatNutrition;

            if (data.ComplexHunger.hunger <= 20f)
                return ActionRunState.Completed;

            if (data.Eatable.NutritionValue > 0)
                return ActionRunState.Continue;

            data.Inventory.Remove(data.Eatable);
            this.instanceHandler.QueueForDestroy(data.Eatable);

            return ActionRunState.Completed;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            this.Disable(agent, ActionDisabler.ForTime(5f));

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
            public ComplexHungerBehaviour ComplexHunger { get; set; }
        }
    }
}
