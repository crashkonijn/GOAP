using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.World
{
    public class HungerSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
            
        }

        public override void Update()
        {
            
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return  (int) references.GetCachedComponent<ComplexHungerBehaviour>().hunger;
        }
    }
}