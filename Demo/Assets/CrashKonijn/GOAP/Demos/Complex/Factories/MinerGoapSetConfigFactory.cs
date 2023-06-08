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
    public class MinerGoapSetConfigFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder(SetIds.Miner);
            
            // Debugger
            builder.SetAgentDebugger<AgentDebugger>();

            // Goals
            builder.AddWanderGoal();
            
            builder.AddFixHungerGoal();
            builder.AddPickupItemGoal<Pickaxe>();

            builder.AddGatherItemGoal<Iron>();
            
            // Actions
            builder.AddWanderAction();

            builder.AddPickupItemAction<Iron>();
            builder.AddPickupItemAction<Pickaxe>();
            builder.AddPickupItemAction<IEatable>();
            
            builder.AddGatherItemAction<Iron, Pickaxe>();
            builder.AddGatherItemSlowAction<Iron>();

            builder.AddEatAction();
            
            // TargetSensors
            builder.AddWanderTargetSensor();
            builder.AddTransformTargetSensor();
            
            builder.AddClosestItemTargetSensor<Iron>();
            builder.AddClosestItemTargetSensor<Pickaxe>();
            builder.AddClosestItemTargetSensor<IEatable>();
            
            builder.AddClosestSourceTargetSensor<Iron>();

            // WorldSensors
            builder.AddIsHoldingSensor<Pickaxe>();
            builder.AddIsHoldingSensor<Iron>();
            builder.AddIsHoldingSensor<IEatable>();
            
            builder.AddIsInWorldSensor<Pickaxe>();
            builder.AddIsInWorldSensor<Iron>();
            builder.AddIsInWorldSensor<IEatable>();
            
            return builder.Build();
        }
    }
}