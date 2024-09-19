using CrashKonijn.Docs.GettingStarted.Actions;
using CrashKonijn.Docs.GettingStarted.Sensors;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Docs.GettingStarted.Capabilities
{
    public class PearCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("PearCapability");

            builder.AddGoal<PickupPearGoal>()
                .AddCondition<PearCount>(Comparison.GreaterThanOrEqual, 3);
            
            builder.AddAction<PickupPearAction>()
                .AddEffect<PearCount>(EffectType.Increase)
                .SetTarget<ClosestPear>();

            builder.AddMultiSensor<PearSensor>();

            return builder.Build();
        }
    }
}