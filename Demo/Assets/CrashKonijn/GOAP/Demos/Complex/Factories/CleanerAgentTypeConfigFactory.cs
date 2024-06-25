using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Demos.Complex.Sensors.World;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class CleanerAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.Cleaner);
            
            builder.AddCapability<BaseCapability>();
            builder.AddCapability<WanderCapability>();
            builder.AddCapability<HungerCapability>();

            builder.CreateCapability("CleanCapability", (capability) =>
            {
                capability.AddGoal<CleanItemsGoal>()
                    .AddCondition<ItemsOnFloor>(Comparison.SmallerThanOrEqual, 0);
                
                capability.AddAction<HaulItemAction>()
                    .SetTarget<TransformTarget>()
                    .AddEffect<ItemsOnFloor>(EffectType.Decrease)
                    .AddCondition<ItemsOnFloor>(Comparison.GreaterThanOrEqual, 1)
                    .SetMoveMode(ActionMoveMode.PerformWhileMoving);
                
                capability.AddWorldSensor<ItemOnFloorSensor>()
                    .SetKey<ItemsOnFloor>();
            });

            return builder.Build();
        }
    }
}
