using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs.Interfaces
{
    public interface IActionConfig : IClassConfig
    {
        int BaseCost { get; }
        ITargetKey Target { get; }
        float InRange { get; }
        ICondition[] Conditions { get; }
        IEffect[] Effects { get; }
        public ActionMoveMode MoveMode { get; }
    }
}