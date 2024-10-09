using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Factories
{
    [CreateAssetMenu(menuName = "Goap/Custom/BaseCapabilityFactory")]
    public class BaseCapabilityFactory : ScriptableCapabilityFactoryBase
    {
        public override ICapabilityConfig Create()
        {
            var builder = new CapabilityBuilder("BaseCapability");

            builder.AddTargetSensor<AgentSensor>()
                .SetTarget<AgentTarget>();

            return builder.Build();
        }
    }
}