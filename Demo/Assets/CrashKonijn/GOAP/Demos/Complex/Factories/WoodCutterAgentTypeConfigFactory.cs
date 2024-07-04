using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class WoodCutterAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.WoodCutter);
            
            builder.AddCapability<BaseCapability>();
            builder.AddCapability<WanderCapability>();
            builder.AddCapability<HungerCapability>();
            
            builder.CreateCapability("WoodCutterCapability", (capability) =>
            {
                // Goals
                capability.AddPickupItemGoal<Axe>();
                capability.AddGatherItemGoal<Wood>();
                
                // Actions
                capability.AddPickupItemAction<Axe>();
                
                capability.AddGatherItemAction<Wood, Axe>();
                capability.AddGatherItemSlowAction<Wood>();
                
                // Target sensors
                capability.AddClosestItemTargetSensor<Axe>();
                capability.AddClosestItemTargetSensor<Wood>();
                
                capability.AddClosestSourceTargetSensor<Wood>();
                
                // World sensors
                capability.AddIsHoldingSensor<Axe>();
                capability.AddIsHoldingSensor<Wood>();
                
                capability.AddIsInWorldSensor<Axe>();
                capability.AddIsInWorldSensor<Wood>();
            });
            
            return builder.Build();
        }
    }
}