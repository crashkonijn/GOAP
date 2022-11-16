using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IActionBase : IAction
    {
        public ActionConfig Config { get; }
        public void SetConfig(ActionConfig config);
        public float GetDistanceCost(ITarget currentTarget, ITarget otherTarget);
        public int GetCost(IWorldData data);

        public IActionData GetData();
        public ActionRunState Perform(Agent agent, IActionData data);
        public void OnStart(Agent agent, IActionData data);
        public void OnEnd(Agent agent, IActionData data);
    }
}