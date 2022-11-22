using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.Actions
{
    [CreateAssetMenu(menuName = "Goap/Actions/WanderAction")]
    public class WanderAction : ActionBase<WanderAction.Data>
    {
        public override Data CreateData()
        {
            return new Data();
        }

        public override void OnStart(IMonoAgent agent, Data data)
        {
            
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data)
        {
            return ActionRunState.Stop;
        }

        public override void OnEnd(IMonoAgent agent, Data data)
        {
            
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }
    }
}