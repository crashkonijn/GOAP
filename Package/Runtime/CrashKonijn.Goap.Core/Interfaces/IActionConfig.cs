using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IActionConfig : IClassConfig
    {
        int BaseCost { get; }
        ITargetKey Target { get; }
        float InRange { get; }
        ICondition[] Conditions { get; }
        IEffect[] Effects { get; }
        public ActionMoveMode MoveMode { get; }
        public IActionProperties Properties { get; }
    }
}