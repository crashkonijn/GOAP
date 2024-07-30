using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Capabilities
{
    public class WanderCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("WanderCapability");

            builder.AddGoal<WanderGoal>()
                .SetBaseCost(50)
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);
            
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetProperties(new WanderAction.Props
                {
                    minTimer = 0.3f,
                    maxTimer = 1f
                });
            
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            return builder.Build();
        }
    }
}