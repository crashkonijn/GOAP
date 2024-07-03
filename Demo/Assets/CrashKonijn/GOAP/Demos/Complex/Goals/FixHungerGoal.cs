using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Goals
{
    public class FixHungerGoal : GoalBase
    {
        public override float GetCost(IActionReceiver agent, IComponentReference references)
        {
            var hunger = references.GetCachedComponent<ComplexHungerBehaviour>().hunger;
            var limited = Mathf.Clamp(hunger, 0f, 100f); // Clamp between 0 and 100
            var normalized = limited / 100f; // Normalize between 0 and 1
            var curved = this.ExponentialCurve(normalized); // Apply exponential curve
            var inverse = 1f - curved; // Inverse
            
            return 50 * inverse;
        }
        
        private float ExponentialCurve(float t)
        {
            float sqr = t * t;
            return sqr / (2.0f * (sqr - t) + 1.0f);
        }
    }
}