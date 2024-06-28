using System;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Goap;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class CreateItemAction<TCreatable> : GoapActionBase<CreateItemAction<TCreatable>.Data, CreateItemAction<TCreatable>.Props>, IInjectable
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
            data.WaitState = ActionRunState.Wait(5f);
        }

        public override void BeforePerform(IMonoAgent agent, Data data)
        {
            this.RemoveRequiredResources(data);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.WaitState.IsRunning())
            {
                return data.WaitState;
            }
            
            return ActionRunState.Completed;
        }

        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
            var item = this.itemFactory.Instantiate<TCreatable>();
            item.transform.position = this.GetRandomPosition(agent);
        }

        private void RemoveRequiredResources(Data data)
        {
            for (var i = 0; i < this.Properties.requiredIron; i++)
            {
                var iron = data.Inventory.Get<Iron>().FirstOrDefault();
                data.Inventory.Remove(iron);
                this.instanceHandler.QueueForDestroy(iron);
            }
            
            for (var j = 0; j < this.Properties.requiredWood; j++)
            {
                var wood = data.Inventory.Get<Wood>().FirstOrDefault();
                data.Inventory.Remove(wood);
                this.instanceHandler.QueueForDestroy(wood);
            }
        }
        
        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return agent.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
        
        [Serializable]
        public class Props : IActionProperties
        {
            public int requiredWood;
            public int requiredIron;
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