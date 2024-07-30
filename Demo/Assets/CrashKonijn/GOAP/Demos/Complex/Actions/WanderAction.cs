using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class WanderAction : GoapActionBase<WanderAction.Data, WanderAction.Props>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            var wait = Random.Range(this.Properties.minTimer, this.Properties.maxTimer);
            
            data.Timer = ActionRunState.Wait(wait);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (data.Timer.IsRunning())
                return data.Timer;
            
            return ActionRunState.Completed;
        }

        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        [Serializable]
        public class Props : IActionProperties
        {
            public float minTimer;
            public float maxTimer;
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IActionRunState Timer { get; set; }
        }
    }
}