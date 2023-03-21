using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace Demos.Complex.Actions
{
    public class GatherItemAction<TGatherable> : ActionBase<GatherItemAction<TGatherable>.Data>
    {
        public override void OnStart(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
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