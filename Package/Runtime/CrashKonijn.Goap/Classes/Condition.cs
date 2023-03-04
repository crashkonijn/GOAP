using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class Condition : ICondition
    {
        public IWorldKey WorldKey { get; set; }
        public bool Positive { get; set; }
    }
}