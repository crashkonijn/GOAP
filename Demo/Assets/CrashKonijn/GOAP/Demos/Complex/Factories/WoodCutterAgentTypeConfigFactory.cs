using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class WoodCutterAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.WoodCutter);
            
            // Goals
            builder.AddWanderGoal();
            
            builder.AddFixHungerGoal();
            builder.AddPickupItemGoal<Axe>();

            builder.AddGatherItemGoal<Wood>();
            
            // Actions
            builder.AddWanderAction();

            builder.AddPickupItemAction<Wood>();
            builder.AddPickupItemAction<Axe>();
            builder.AddPickupItemAction<IEatable>();
            
            builder.AddGatherItemAction<Wood, Axe>();
            builder.AddGatherItemSlowAction<Wood>();

            builder.AddEatAction();
            
            // TargetSensors
            builder.AddWanderTargetSensor();
            builder.AddTransformTargetSensor();
            
            builder.AddClosestItemTargetSensor<Axe>();
            builder.AddClosestItemTargetSensor<Wood>();
            builder.AddClosestItemTargetSensor<IEatable>();
            
            builder.AddClosestSourceTargetSensor<Wood>();

            // WorldSensors
            builder.AddIsHoldingSensor<Axe>();
            builder.AddIsHoldingSensor<Wood>();
            builder.AddIsHoldingSensor<IEatable>();
            
            builder.AddIsInWorldSensor<Axe>();
            builder.AddIsInWorldSensor<Wood>();
            builder.AddIsInWorldSensor<IEatable>();
            
            return builder.Build();
        }
    }
}