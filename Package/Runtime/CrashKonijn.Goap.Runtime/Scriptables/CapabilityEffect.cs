using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class CapabilityEffect
    {
        public ClassRef worldKey = new();
        public EffectType effect;

        public override string ToString() => $"{this.worldKey.Name}{this.effect.ToName()}";
    }
}