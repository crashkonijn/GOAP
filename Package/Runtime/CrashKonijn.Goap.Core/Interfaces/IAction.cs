using System;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAction : IConnectable, IHasConfig<IActionConfig>
    {
        IActionProperties Properties { get; }
        float GetCost(IMonoAgent agent, IComponentReference references);
        [Obsolete("Please use the IsInRange method")]
        float GetInRange(IMonoAgent agent, IActionData data);
        bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references);
        IActionData GetData();
        void Created();
        IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context);
        void Start(IMonoAgent agent, IActionData data);
        void Stop(IMonoAgent agent, IActionData data);
        void Complete(IMonoAgent agent, IActionData data);
    }
}