using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class CapabilityCondition
    {
        public ClassRef worldKey = new();
        public Comparison comparison;
        public int amount;

        public CapabilityCondition() { }

        public CapabilityCondition(string data)
        {
            var split = data.Split(' ');
            this.worldKey.Name = split[0];
            this.comparison = split[1].FromName();
            this.amount = int.Parse(split[2]);
        }

        public override string ToString() => $"{this.worldKey.Name} {this.comparison.ToName()} {this.amount}";
    }
}
