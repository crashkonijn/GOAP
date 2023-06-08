using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using Demos.Complex.Classes;
using Demos.Complex.Classes.Items;
using Demos.Complex.Factories.Extensions;
using Demos.Complex.Interfaces;
using Demos.Shared;

namespace Demos.Complex.Factories
{
    public class WoodCutterGoapSetConfigFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder(SetIds.WoodCutter);
            
            // Debugger
            builder.SetAgentDebugger<AgentDebugger>();

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