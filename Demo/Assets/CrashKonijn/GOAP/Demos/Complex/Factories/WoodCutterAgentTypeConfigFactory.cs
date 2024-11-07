using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Factories.Capabilities;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Sensors.Multi;
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
                capability.AddClosestSourceTargetSensor<Wood>();

                // World sensors
                capability.AddIsHoldingSensor<Axe>();
                capability.AddIsHoldingSensor<Wood>();

                // Multi sensor
                capability.AddMultiSensor<ItemSensor<Axe>>();
                capability.AddMultiSensor<ItemSensor<Wood>>();
            });

            return builder.Build();
        }
    }
}
