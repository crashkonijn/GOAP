using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Sensors;

namespace Demos.Sensors.World
{
    public class IsAliveSensor : LocalWorldSensorBase
    {
        public override void Update()
        {
            
        }

        public override bool Sense(IMonoAgent agent)
        {
            return true;
        }
    }
}