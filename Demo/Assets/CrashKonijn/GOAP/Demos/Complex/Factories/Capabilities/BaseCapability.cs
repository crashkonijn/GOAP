using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Factories.Extensions;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using TransformTarget = CrashKonijn.Goap.Demos.Complex.Targets.TransformTarget;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Capabilities
{
    public class BaseCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("BaseCapability");

            builder.AddTargetSensor<TransformSensor>()
                .SetTarget<TransformTarget>();

            return builder.Build();
        }
    }
}