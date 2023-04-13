using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class ActionConfig : IActionConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public int BaseCost { get; set; }
        public ITargetKey Target { get; set; }
        public float InRange { get; set; }
        public ICondition[] Conditions { get; set; }
        public IEffect[] Effects { get; set; }
    }
}