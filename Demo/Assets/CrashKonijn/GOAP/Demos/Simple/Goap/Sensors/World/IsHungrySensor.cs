using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.World
{
    [GoapId("Simple-IsHungrySensor")]
    public class IsHungrySensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var hungerBehaviour = references.GetCachedComponent<HungerBehaviour>();

            if (hungerBehaviour == null)
                return false;

            return hungerBehaviour.hunger > 20;
        }
    }
}