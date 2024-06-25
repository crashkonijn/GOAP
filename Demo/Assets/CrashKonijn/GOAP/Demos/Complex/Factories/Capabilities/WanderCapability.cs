using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Capabilities
{
    public class WanderCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("WanderCapability");

            builder.AddGoal<WanderGoal>()
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