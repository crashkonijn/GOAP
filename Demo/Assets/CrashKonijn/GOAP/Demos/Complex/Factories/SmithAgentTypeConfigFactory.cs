using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Classes.Sources;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;

namespace CrashKonijn.Goap.Demos.Complex.Factories
{
    public class SmithAgentTypeConfigFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var builder = new AgentTypeBuilder(SetIds.Smith);
            
            builder.AddCapability<BaseCapability>();
            builder.AddCapability<WanderCapability>();
            builder.AddCapability<HungerCapability>();

            builder.CreateCapability("SmithCapability", (capability) =>
            {
                // Goals
                capability.AddCreateItemGoal<Axe>();
                capability.AddCreateItemGoal<Pickaxe>();

                // Actions
                capability.AddPickupItemAction<Wood>();
                capability.AddPickupItemAction<Iron>();

                capability.AddCreateItemAction<Pickaxe>(2, 1);
                capability.AddCreateItemAction<Axe>(1, 2);

                // TargetSensors
                capability.AddClosestObjectTargetSensor<AnvilSource>();

                capability.AddClosestItemTargetSensor<Iron>();
                capability.AddClosestItemTargetSensor<Wood>();

                capability.AddClosestSourceTargetSensor<Iron>();
                capability.AddClosestSourceTargetSensor<Wood>();

                // WorldSensors
                capability.AddIsHoldingSensor<Wood>();
                capability.AddIsHoldingSensor<Iron>();

                capability.AddIsInWorldSensor<Axe>();
                capability.AddIsInWorldSensor<Pickaxe>();
                capability.AddIsInWorldSensor<Wood>();
                capability.AddIsInWorldSensor<Iron>();
            });

            return builder.Build();
        }
    }
}