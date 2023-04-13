using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Simple.Behaviours;

namespace Demos.Simple.Actions
{
    public class PluckAppleAction : ActionBase<PluckAppleAction.Data>
    {
        public override void Created()
        {
        }
        
        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget transformTarget)
                return;
            
            data.Tree =  transformTarget.Transform.GetComponent<TreeBehaviour>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Tree == null)
                return ActionRunState.Stop;

            data.Progress += context.DeltaTime * 10f;

            if (data.Progress < 10)
                return ActionRunState.Continue;
            
            var apple = data.Tree.DropApple();
            var inventory = agent.GetComponent<InventoryBehaviour>();
            
            if (inventory != null)
                inventory.Put(apple);
            
            return ActionRunState.Stop;
        }
        
        public override void End(IMonoAgent agent, Data data)
        {
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public TreeBehaviour Tree { get; set; }
            public float Progress { get; set; } = 0f;
        }
    }
}