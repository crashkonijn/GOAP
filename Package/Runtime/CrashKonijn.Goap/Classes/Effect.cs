using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class Effect : IEffect
    {
        public IWorldKey WorldKey { get; set; }
        public bool Increase { get; set; }
    }
}