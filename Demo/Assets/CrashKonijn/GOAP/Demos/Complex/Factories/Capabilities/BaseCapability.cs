using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Capabilities
{
    public class BaseCapability : CapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("BaseCapability");

            builder.AddTargetSensor<TransformSensor>()
                .SetTarget<Targets.TransformTarget>();

            return builder.Build();
        }
    }
}