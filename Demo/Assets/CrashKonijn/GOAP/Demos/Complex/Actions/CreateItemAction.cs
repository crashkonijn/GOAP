using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Classes.Items;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Actions
{
    public class CreateItemAction<TCreatable> : ActionBase<CreateItemAction<TCreatable>.Data>, IInjectable
        where TCreatable : ItemBase, ICreatable
    {
        private ItemFactory itemFactory;
        private InstanceHandler instanceHandler;

        public void Inject(GoapInjector injector)
        {
            this.itemFactory = injector.itemFactory;
            this.instanceHandler = injector.instanceHandler;
        }

        public override void Created()
        {
        }
        
        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = 5f;
            data.RequiredWood = this.GetRequiredWood();
            data.RequiredIron = this.GetRequiredIron();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.State == State.NotStarted)
            {
                data.State = State.Started;
                this.RemoveRequiredResources(data);
            }
            
            data.Timer -= context.DeltaTime;
            
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            var item = this.itemFactory.Instantiate<TCreatable>();
            item.transform.position = this.GetRandomPosition(agent);
            
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }

        private void RemoveRequiredResources(Data data)
        {
            for (var i = 0; i < data.RequiredIron; i++)
            {
                var iron = data.Inventory.Get<Iron>().FirstOrDefault();
                data.Inventory.Remove(iron);
                this.instanceHandler.QueueForDestroy(iron);
            }
            
            for (var j = 0; j < data.RequiredWood; j++)
            {
                var wood = data.Inventory.Get<Wood>().FirstOrDefault();
                data.Inventory.Remove(wood);
                this.instanceHandler.QueueForDestroy(wood);
            }
        }

        private int GetRequiredWood()
        {
            return this.Config.Conditions.FirstOrDefault(x => x.WorldKey.Name == "IsHolding<Wood>")!.Amount;
        }

        private int GetRequiredIron()
        {
            return this.Config.Conditions.FirstOrDefault(x => x.WorldKey.Name == "IsHolding<Iron>")!.Amount;
        }
        
        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return agent.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public int RequiredWood { get; set; }
            public int RequiredIron { get; set; }
            public State State { get; set; } = State.NotStarted;
            public float Timer { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }

        public enum State
        {
            NotStarted,
            Started
        }
    }
}