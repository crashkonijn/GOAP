using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs;
using Demos.Complex.Classes.Items;
using Demos.Complex.Interfaces;

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
            builder.AddWanderTargetSensor();
            builder.AddTransformTargetSensor();
            
            builder.AddClosestItemTargetSensor<Axe>();
            builder.AddClosestItemTargetSensor<Iron>();
            builder.AddClosestItemTargetSensor<Pickaxe>();
            builder.AddClosestItemTargetSensor<Wood>();
            builder.AddClosestItemTargetSensor<IEatable>();
            
            builder.AddClosestSourceTargetSensor<Iron>();
            builder.AddClosestSourceTargetSensor<Wood>();

            // WorldSensors
            builder.AddIsHoldingSensor<Axe>();
            builder.AddIsHoldingSensor<Pickaxe>();
            builder.AddIsHoldingSensor<Wood>();
            builder.AddIsHoldingSensor<Iron>();
            builder.AddIsHoldingSensor<IEatable>();
            
            builder.AddIsInWorldSensor<IEatable>();
            
            builder.AddItemOnFloorSensor();

            return builder.Build();
        }
    }
}