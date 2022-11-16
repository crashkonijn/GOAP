using CrashKonijn.Goap.Behaviours;
using Demos.Behaviours;

namespace Demos.Sensors.World
{
    public class IsHungrySensor : LocalWorldSensorBase
    {
        public override bool Sense(Agent agent)
        {
            var hungerBehaviour = agent.GetComponent<HungerBehaviour>();

            if (hungerBehaviour == null)
                return false;

            return hungerBehaviour.hunger > 20;
        }
    }
}