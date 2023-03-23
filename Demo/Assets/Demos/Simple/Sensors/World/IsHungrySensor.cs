using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;
using Demos.Shared.Behaviours;
using Demos.Simple.Behaviours;

namespace Demos.Simple.Sensors.World
{
    public class IsHungrySensor : LocalWorldSensorBase
    {
        public override void Update()
        {
            
        }

        public override SenseValue Sense(IMonoAgent agent)
        {
            var hungerBehaviour = agent.GetComponent<HungerBehaviour>();

            if (hungerBehaviour == null)
                return false;

            return hungerBehaviour.hunger > 20;
        }
    }
}