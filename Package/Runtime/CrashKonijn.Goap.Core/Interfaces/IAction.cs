using System;
using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAction : IConnectable, IHasConfig<IActionConfig>
    {
        float GetCost(IMonoAgent agent, IComponentReference references);
        [Obsolete("Please use the IsInRange method")]
        float GetInRange(IMonoAgent agent, IActionData data);
        bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references);
        IActionData GetData();
        void Created();
        public ActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context);
        void Start(IMonoAgent agent, IActionData data);
        void End(IMonoAgent agent, IActionData data);
    }
}