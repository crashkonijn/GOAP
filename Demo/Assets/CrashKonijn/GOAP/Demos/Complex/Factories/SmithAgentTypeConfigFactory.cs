using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Classes.Sources;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Sensors.Multi;
using CrashKonijn.Goap.Runtime;

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

                capability.AddClosestSourceTargetSensor<Iron>();
                capability.AddClosestSourceTargetSensor<Wood>();

                // WorldSensors
                capability.AddIsHoldingSensor<Wood>();
                capability.AddIsHoldingSensor<Iron>();

                // Multi sensor
                capability.AddMultiSensor<ItemSensor<Axe>>();
                capability.AddMultiSensor<ItemSensor<Pickaxe>>();
                capability.AddMultiSensor<ItemSensor<Wood>>();
                capability.AddMultiSensor<ItemSensor<Iron>>();
            });

            return builder.Build();
        }
    }
}
