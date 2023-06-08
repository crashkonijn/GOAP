using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using Demos.Complex.Classes;
using Demos.Complex.Factories.Extensions;
using Demos.Complex.Interfaces;
using Demos.Shared;

namespace Demos.Complex.Factories
{
    public class CleanerGoapSetConfigFactory : GoapSetFactoryBase
    {
        public override IGoapSetConfig Create()
        {
            var builder = new GoapSetBuilder(SetIds.Cleaner);
            
            // Debugger
            builder.SetAgentDebugger<AgentDebugger>();

            // Goals
            builder.AddWanderGoal();
            
            builder.AddCleanItemsGoal();
            builder.AddFixHungerGoal();

            // Actions
            builder.AddWanderAction();

            builder.AddHaulItemAction();
            builder.AddPickupItemAction<IEatable>();
            builder.AddEatAction();
            
            // TargetSensors
            builder.AddWanderTargetSensor();
            builder.AddTransformTargetSensor();
            builder.AddClosestItemTargetSensor<IEatable>();
            
            // WorldSensors
            builder.AddIsHoldingSensor<IEatable>();
            
            builder.AddIsInWorldSensor<IEatable>();
            
            builder.AddItemOnFloorSensor();

            return builder.Build();
        }
    }
}