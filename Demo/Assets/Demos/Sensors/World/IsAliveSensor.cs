using CrashKonijn.Goap.Behaviours;

namespace Demos.Sensors.World
{
    public class IsAliveSensor : LocalWorldSensorBase
    {
        public override bool Sense(Agent agent)
        {
            return true;
        }
    }
}