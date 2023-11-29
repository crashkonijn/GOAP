using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Classes.RunStates;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Actions
{
    [GoapId("Simple-PluckAppleAction")]
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

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Tree == null)
                return ActionRunState.Stop;

            data.Progress += context.DeltaTime * 10f;

            if (data.Progress < 10)
                return ActionRunState.Continue;
            
            var apple = data.Tree.DropApple();
            
            data.Inventory.Put(apple);
            
            return ActionRunState.Completed;
        }
        
        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public TreeBehaviour Tree { get; set; }
            public float Progress { get; set; } = 0f;
            
            [GetComponent]
            public InventoryBehaviour Inventory { get; set; }
        }
    }
}