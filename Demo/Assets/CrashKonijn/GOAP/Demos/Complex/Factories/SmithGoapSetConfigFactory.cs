using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using Demos.Complex.Classes;
using Demos.Complex.Classes.Items;
using Demos.Complex.Classes.Sources;
using Demos.Complex.Factories.Extensions;
using Demos.Complex.Interfaces;
using Demos.Shared;

namespace Demos.Complex.Factories
{
    public class SmithGoapSetConfigFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder(SetIds.Smith);
            
            // Debugger
            builder.SetAgentDebugger<AgentDebugger>();

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