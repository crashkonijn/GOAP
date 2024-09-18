using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use GoapActionBase instead of ActionBase")]
    public abstract class ActionBase<TActionData> : GoapActionBase<TActionData, EmptyActionProperties>
        where TActionData : IActionData, new()
    {
        public override IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context)
        {
            return this.Perform(agent, data, context as ActionContext);
        }

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data, ActionContext context);
    }
}