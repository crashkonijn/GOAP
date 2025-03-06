using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Actions;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using CrashKonijn.Goap.Demos.Complex.Sensors.World;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

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
                    .SetBaseCost(20)
                    .AddCondition<ItemsOnFloor>(Comparison.SmallerThanOrEqual, 0);

                capability.AddAction<HaulItemAction>()
                    .SetTarget<HaulTarget>()
                    .AddEffect<ItemsOnFloor>(EffectType.Decrease)
                    .AddCondition<ItemsOnFloor>(Comparison.GreaterThanOrEqual, 1)
                    .SetMoveMode(ActionMoveMode.PerformWhileMoving)
                    .SetValidateConditions(false)
                    .SetCallback((action) =>
                    {
                        Debug.Log($"Action callback: {action}");
                    });

                capability.AddWorldSensor<ItemOnFloorSensor>()
                    .SetKey<ItemsOnFloor>();

                capability.AddTargetSensor<HaulTargetSensor>()
                    .SetTarget<HaulTarget>();
            });

            return builder.Build();
        }
    }
}
