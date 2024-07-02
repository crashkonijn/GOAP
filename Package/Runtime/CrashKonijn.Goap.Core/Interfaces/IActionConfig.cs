using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IActionConfig : IClassConfig
    {
        int BaseCost { get; }
        ITargetKey Target { get; }
        float StoppingDistance { get; }
        bool RequiresTarget { get; }
        ICondition[] Conditions { get; }
        IEffect[] Effects { get; }
        public ActionMoveMode MoveMode { get; }
        public IActionProperties Properties { get; }
    }
}