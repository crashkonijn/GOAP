using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.Simple.Actions
{
    public class WanderAction : ActionBase<WanderAction.Data>
    {
        public override Data CreateData()
        {
            return new Data
            {
                Timer = Random.Range(0.3f, 1f)
            };
        }

        public override void OnStart(IMonoAgent agent, Data data)
        {
            
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data)
        {
            data.Timer -= Time.deltaTime;
            
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            return ActionRunState.Stop;
        }

        public override void OnEnd(IMonoAgent agent, Data data)
        {
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public float Timer { get; set; }
        }
    }
}