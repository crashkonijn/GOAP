using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Resolver;
using Demos.Complex.Actions;
using Demos.Complex.Classes;
using Demos.Complex.Classes.Items;
using Demos.Complex.Goals;
using Demos.Complex.Sensors.Target;
using Demos.Complex.Sensors.World;
using Demos.Shared.Actions;
using Demos.Shared.Goals;
using Demos.Shared.Sensors.Target;
using Demos.Simple.Sensors.Target;

namespace Demos.Complex.Behaviours
{
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder("ComplexSet");

            // Goals
            builder.AddGoal<WanderGoal>()
                .AddCondition(WorldKeys.IsWandering, Comparison.GreaterThanOrEqual, 1);
            
            builder.AddGoal<CreateItemGoal<Axe>>()
                .AddCondition<Axe>(WorldKeys.CreatedItem, Comparison.GreaterThanOrEqual, 1);
            
            builder.AddGoal<CreateItemGoal<Pickaxe>>()
                .AddCondition<Pickaxe>(WorldKeys.CreatedItem, Comparison.GreaterThanOrEqual, 1);
            
            // Actions
            builder.AddAction<WanderAction>()
                .SetTarget(Targets.WanderTarget)
                .AddEffect(WorldKeys.IsWandering, true);

            builder.AddAction<PickupItemAction<Wood>>()
                .SetTarget<Wood>(Targets.ClosestTarget)
                .AddEffect<Wood>(WorldKeys.IsHolding, true);

            builder.AddAction<PickupItemAction<Iron>>()
                .SetTarget<Iron>(Targets.ClosestTarget)
                .AddEffect<Iron>(WorldKeys.IsHolding, true);

            builder.AddAction<CreateItemAction<Pickaxe>>()
                .SetTarget(Targets.TransformTarget)
                .AddEffect<Pickaxe>(WorldKeys.CreatedItem, true)
                .AddCondition<Iron>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 1)
                .AddCondition<Wood>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 2);
            
            builder.AddAction<CreateItemAction<Axe>>()
                .SetTarget(Targets.TransformTarget)
                .AddEffect<Axe>(WorldKeys.CreatedItem, true)
                .AddCondition<Iron>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 2)
                .AddCondition<Wood>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 1);
            
            // TargetSensors
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget(Targets.WanderTarget);

            builder.AddTargetSensor<TransformSensor>()
                .SetTarget(Targets.TransformTarget);

            builder.AddTargetSensor<ClosestItemSensor<Axe>>()
                .SetTarget<Axe>(Targets.ClosestTarget);
            builder.AddTargetSensor<ClosestItemSensor<Iron>>()
                .SetTarget<Iron>(Targets.ClosestTarget);
            builder.AddTargetSensor<ClosestItemSensor<Pickaxe>>()
                .SetTarget<Pickaxe>(Targets.ClosestTarget);
            builder.AddTargetSensor<ClosestItemSensor<Wood>>()
                .SetTarget<Wood>(Targets.ClosestTarget);

            // WorldSensors
            builder.AddWorldSensor<IsHoldingSensor<Axe>>()
                .SetKey<Axe>(WorldKeys.IsHolding);
            builder.AddWorldSensor<IsHoldingSensor<Pickaxe>>()
                .SetKey<Pickaxe>(WorldKeys.IsHolding);
            builder.AddWorldSensor<IsHoldingSensor<Wood>>()
                .SetKey<Wood>(WorldKeys.IsHolding);
            builder.AddWorldSensor<IsHoldingSensor<Iron>>()
                .SetKey<Iron>(WorldKeys.IsHolding);
            
            // builder.AddWorldSensor<CanCreateSensor<Axe>>();
            // builder.AddWorldSensor<CanCreateSensor<Pickaxe>>();

            return builder.Build();
        }
    }
}