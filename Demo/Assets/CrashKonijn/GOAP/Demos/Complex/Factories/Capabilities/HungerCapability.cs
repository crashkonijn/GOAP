using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;
using TransformTarget = CrashKonijn.Goap.Demos.Complex.Targets.TransformTarget;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Capabilities
{
    public class HungerCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("HungerCapability");

            // Goals
            builder.AddGoal<FixHungerGoal>()
                .AddCondition<IsHungry>(Comparison.SmallerThanOrEqual, 0);
            
            // Actions
            builder.AddAction<EatAction>()
                .SetTarget<TransformTarget>()
                .AddEffect<IsHungry>(EffectType.Decrease)
                .AddCondition<IsHolding<IEatable>>(Comparison.GreaterThanOrEqual, 1);
            
            builder.AddAction<GatherItemAction<Apple>>()
                .SetTarget<ClosestSourceTarget<Apple>>()
                .AddEffect<IsHolding<IEatable>>(EffectType.Increase)
                .SetBaseCost(3)
                .SetProperties(new GatherItemAction<Apple>.Props
                {
                    pickupItem = true,
                    timer = 3
                });
            
            builder.AddPickupItemAction<IEatable>();
            
            // Target sensors
            builder.AddClosestItemTargetSensor<IEatable>();
            builder.AddClosestSourceTargetSensor<Apple>();
            
            // World sensors
            builder.AddIsHoldingSensor<IEatable>();
            builder.AddIsInWorldSensor<IEatable>();

            return builder.Build();
        }
    }
}