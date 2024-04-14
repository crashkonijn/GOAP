using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Classes.Sources;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class SmithAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.Smith);

            // Goals
            builder.AddWanderGoal();
            
            builder.AddCreateItemGoal<Axe>();
            builder.AddCreateItemGoal<Pickaxe>();
            builder.AddFixHungerGoal();

            // Actions
            builder.AddWanderAction();

            builder.AddPickupItemAction<Wood>();
            builder.AddPickupItemAction<Iron>();
            builder.AddPickupItemAction<IEatable>();
            
            builder.AddCreateItemAction<Pickaxe>();
            builder.AddCreateItemAction<Axe>();

            builder.AddEatAction();
            
            // TargetSensors
            builder.AddWanderTargetSensor();
            builder.AddTransformTargetSensor();
            
            builder.AddClosestObjectTargetSensor<AnvilSource>();
            
            builder.AddClosestItemTargetSensor<Iron>();
            builder.AddClosestItemTargetSensor<Wood>();
            builder.AddClosestItemTargetSensor<IEatable>();
            
            builder.AddClosestSourceTargetSensor<Iron>();
            builder.AddClosestSourceTargetSensor<Wood>();

            // WorldSensors
            builder.AddIsHoldingSensor<Wood>();
            builder.AddIsHoldingSensor<Iron>();
            builder.AddIsHoldingSensor<IEatable>();
            
            builder.AddIsInWorldSensor<Axe>();
            builder.AddIsInWorldSensor<Pickaxe>();
            builder.AddIsInWorldSensor<Wood>();
            builder.AddIsInWorldSensor<Iron>();
            builder.AddIsInWorldSensor<IEatable>();
            
            builder.AddItemOnFloorSensor();

            return builder.Build();
        }
    }
}