using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class MinerAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.Miner);
            
            builder.AddCapability<BaseCapability>();
            builder.AddCapability<WanderCapability>();
            builder.AddCapability<HungerCapability>();
            
            builder.CreateCapability("MineCapability", (capability) =>
            {
                capability.AddPickupItemGoal<Pickaxe>();
                capability.AddGatherItemGoal<Iron>();
                
                capability.AddPickupItemAction<Pickaxe>();
                
                capability.AddGatherItemAction<Iron, Pickaxe>();
                capability.AddGatherItemSlowAction<Iron>();
                
                capability.AddClosestItemTargetSensor<Iron>();
                capability.AddClosestItemTargetSensor<Pickaxe>();
                
                capability.AddClosestSourceTargetSensor<Iron>();
                
                capability.AddIsHoldingSensor<Pickaxe>();
                capability.AddIsHoldingSensor<Iron>();
                
                capability.AddIsInWorldSensor<Pickaxe>();
                capability.AddIsInWorldSensor<Iron>();
            });
            
            return builder.Build();
        }
    }
}