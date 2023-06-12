using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Classes.Sources;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Actions
{
    public class HaulItemAction : ActionBase<HaulItemAction.Data>, IInjectable
    {
        private ItemCollection itemCollection;

        public void Inject(GoapInjector injector)
        {
            this.itemCollection = injector.itemCollection;
        }

        public override void Created()
        {
        }
        
        public override void Start(IMonoAgent agent, Data data)
        {
            var item = this.itemCollection.Closest(agent.transform.position, false, false, false);
            
            if (item is null)
                return;

            item.Claim();
            
            data.Item = item;
            data.Target = new TransformTarget(data.Item.gameObject.transform);
            data.Timer = 0.5f;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Item is null)
                return ActionRunState.Stop;
            
            switch (data.State)
            {
                case State.MovingToItem:
                    return this.MoveToItem(agent, data);
                case State.MovingToTarget:
                    return this.MoveToTarget(data);
                default:
                    return ActionRunState.Stop;
            }
        }

        private ActionRunState MoveToItem(IMonoAgent agent, Data data)
        {
            if (data.Target is null)
                return ActionRunState.Stop;
            
            // Someone else picked the item up
            if (data.Item.IsHeld)
                return ActionRunState.Stop;

            if (data.Timer > 0)
            {
                data.Timer -= Time.deltaTime;
                return ActionRunState.Continue;
            }
            
            // pickup item
            data.Inventory.Hold(data.Item);
            
            // find dropoff target
            var box = this.GetClosestBox(agent, data);
            
            data.Box = box;
            // This will start the movement to the box
            data.Target = new TransformTarget(box.transform);
            data.State = State.MovingToTarget;
            // Dropoff time
            data.Timer = 0.5f;
            
            return ActionRunState.Continue;
        }
        
        private ActionRunState MoveToTarget(Data data)
        {
            if (data.Target is null)
                return ActionRunState.Stop;
            
            if (data.Timer > 0)
            {
                data.Timer -= Time.deltaTime;
                return ActionRunState.Continue;
            }
            
            data.Inventory.Remove(data.Item);
            data.Box.Place(data.Item);
            
            return ActionRunState.Stop;
        }
        
        private BoxSource GetClosestBox(IMonoAgent agent, Data data)
        {
            var boxes = GameObject.FindObjectsOfType<BoxSource>();
            var typeBox = boxes.FirstOrDefault(x => x.ItemType != null && x.ItemType == data.Item.GetType());
            
            if (typeBox != null)
                return typeBox;

            var box = boxes.Where(x => x.ItemType == null).Closest(agent.transform.position);
            box.ItemType = data.Item.GetType();
            
            return box;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IHoldable Item { get; set; }
            public BoxSource Box { get; set; }
            public State State { get; set; } = State.MovingToItem;
            public float Timer { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
        }

        public enum State
        {
            MovingToItem,
            MovingToTarget
        }
    }
}