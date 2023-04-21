using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IActionBase : IAction, IHasConfig<IActionConfig>
    {
        float GetCost(IMonoAgent agent, IComponentReference references);
        float GetInRange(IMonoAgent agent, IActionData data);
        IActionData GetData();
        void Created();
        public ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context);
        void Start(IMonoAgent agent, IActionData data);
        void End(IMonoAgent agent, IActionData data);
    }
}