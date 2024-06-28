using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Runtime;

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

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            var hungerBehaviour = references.GetCachedComponent<SimpleHungerBehaviour>();

            if (hungerBehaviour == null)
                return false;

            return hungerBehaviour.hunger > 20;
        }
    }
}