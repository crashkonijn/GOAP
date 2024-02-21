using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.RunStates;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Actions
{
    public class WanderAction : ActionBase<WanderAction.Data, WanderAction.Props>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = Random.Range(this.Properties.minTimer, this.Properties.maxTimer);
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (!agent.Timers.Action.IsRunningFor(data.Timer))
                return ActionRunState.Wait(data.Timer);
            
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
            public float Timer { get; set; }
        }
    }
}