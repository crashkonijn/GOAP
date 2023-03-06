using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public interface IActionConfig : IClassConfig
    {
        int BaseCost { get; }
        ITargetKey Target { get; }
        float InRange { get; }
        
        ICondition[] Conditions { get; }
        IEffect[] Effects { get; }
    }
}