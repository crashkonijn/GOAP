using CrashKonijn.Docs.GettingStarted.Actions;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Docs.GettingStarted.Capabilities
{
    public class EatCapabilityFactory : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("EatCapability");

            builder.AddGoal<EatGoal>();
            
            builder.AddAction<EatAction>()
                .AddEffect<Hunger>(EffectType.Decrease)
                .AddCondition<PearCount>(Comparison.GreaterThanOrEqual, 1)
                .SetRequiresTarget(false);
            
            return builder.Build();
        }
    }
}