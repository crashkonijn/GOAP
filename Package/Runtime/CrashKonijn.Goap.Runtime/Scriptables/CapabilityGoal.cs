using System;
using System.Collections.Generic;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class CapabilityGoal
    {
        public ClassRef goal = new();

        public float baseCost = 1;
        public List<CapabilityCondition> conditions = new();
    }
}