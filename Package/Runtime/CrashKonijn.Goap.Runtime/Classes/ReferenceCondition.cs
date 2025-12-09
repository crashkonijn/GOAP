using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ReferenceCondition : IReferenceCondition
    {
        public IWorldKey WorldKey { get; set; }
        public Comparison Comparison { get; set; }
        public IWorldKey ValueKey { get; set; }

        public ReferenceCondition()
        {
        }

        public ReferenceCondition(IWorldKey worldKey, Comparison comparison, IWorldKey valueKey)
        {
            this.WorldKey = worldKey;
            this.Comparison = comparison;
            this.ValueKey = valueKey;
        }
    }
}