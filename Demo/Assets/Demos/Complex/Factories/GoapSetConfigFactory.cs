using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Resolver;
using Demos.Complex.Actions;
using Demos.Complex.Classes;
using Demos.Complex.Classes.Items;
using Demos.Complex.Goals;
using Demos.Complex.Interfaces;
using Demos.Complex.Sensors.Target;
using Demos.Complex.Sensors.World;
using Demos.Shared.Actions;
using Demos.Shared.Goals;
using Demos.Shared.Sensors.Target;
using Demos.Simple.Sensors.Target;

namespace Demos.Complex.Factories
{
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder("ComplexSet");

            // Goals
            builder.AddWanderGoal();
            
            builder.AddCreateItemGoal<Axe>();
            builder.AddCreateItemGoal<Pickaxe>();
            builder.AddCleanItemsGoal();
            builder.AddFixHungerGoal();
            
            // Actions
            builder.AddWanderAction();

            builder.AddPickupItemAction<Wood>();
            builder.AddPickupItemAction<Iron>();
            builder.AddPickupItemAction<IEatable>();
            
            builder.AddGatherItemAction<Wood>();
            builder.AddGatherItemAction<Iron>();

            builder.AddCreateItemAction<Pickaxe>();
            builder.AddCreateItemAction<Axe>();

            builder.AddHaulItemAction();

            builder.AddEatAction();
            
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
            builder.AddTargetSensor<ClosestItemSensor<IEatable>>()
                .SetTarget<IEatable>(Targets.ClosestTarget);
            
            builder.AddTargetSensor<ClosestSourceSensor<Iron>>()
                .SetTarget<Iron>(Targets.ClosestSourceTarget);
            
            builder.AddTargetSensor<ClosestSourceSensor<Wood>>()
                .SetTarget<Wood>(Targets.ClosestSourceTarget);

            // WorldSensors
            builder.AddWorldSensor<IsHoldingSensor<Axe>>()
                .SetKey<Axe>(WorldKeys.IsHolding);
            builder.AddWorldSensor<IsHoldingSensor<Pickaxe>>()
                .SetKey<Pickaxe>(WorldKeys.IsHolding);
            builder.AddWorldSensor<IsHoldingSensor<Wood>>()
                .SetKey<Wood>(WorldKeys.IsHolding);
            builder.AddWorldSensor<IsHoldingSensor<Iron>>()
                .SetKey<Iron>(WorldKeys.IsHolding);
            
            builder.AddWorldSensor<IsHoldingSensor<IEatable>>()
                .SetKey<IEatable>(WorldKeys.IsHolding);
            
            builder.AddWorldSensor<ItemOnFloorSensor>()
                .SetKey<Iron>(WorldKeys.ItemsOnFloor);
            
            // builder.AddWorldSensor<CanCreateSensor<Axe>>();
            // builder.AddWorldSensor<CanCreateSensor<Pickaxe>>();

            return builder.Build();
        }
    }
}