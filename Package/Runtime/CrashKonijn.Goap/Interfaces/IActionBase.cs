using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IActionBase : LamosInteractive.Goap.Interfaces.IAction, IHasConfig<IActionConfig>
    {
        public IActionConfig Config { get; }
        public float GetDistanceCost(ITarget currentTarget, ITarget otherTarget);
        public int GetCost(IWorldData data);

        public IActionData GetData();
        public ActionRunState Perform(IMonoAgent agent, IActionData data);
        public void OnStart(IMonoAgent agent, IActionData data);
        public void OnEnd(IMonoAgent agent, IActionData data);
    }
}