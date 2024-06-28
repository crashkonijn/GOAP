using CrashKonijn.Agent.Core;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace Demos.Shared.Actions
{
    public class WanderAction : GoapActionBase<WanderAction.Data>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = Random.Range(0.3f, 1f);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            data.Timer -= context.DeltaTime;
            
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            return ActionRunState.Stop;
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
            public float Timer { get; set; }
        }
    }
}