using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IActionBase : LamosInteractive.Goap.Interfaces.IAction
    {
        public ActionConfig Config { get; }
        public void SetConfig(ActionConfig config);
        public float GetDistanceCost(ITarget currentTarget, ITarget otherTarget);
        public int GetCost(IWorldData data);

        public IActionData GetData();
        public ActionRunState Perform(IMonoAgent agent, IActionData data);
        public void OnStart(IMonoAgent agent, IActionData data);
        public void OnEnd(IMonoAgent agent, IActionData data);
    }
}