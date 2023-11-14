using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class CleanerAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.Cleaner);
            
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