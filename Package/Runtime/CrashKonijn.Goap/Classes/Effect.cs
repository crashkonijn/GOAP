using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class Effect : IEffect
    {
        public IWorldKey WorldKey { get; set; }
        public bool Increase { get; set; }
    }
}