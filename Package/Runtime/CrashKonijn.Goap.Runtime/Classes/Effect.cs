using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class Effect : IEffect
    {
        public IWorldKey WorldKey { get; set; }
        public bool Increase { get; set; }
    }
}