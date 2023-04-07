using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Sensors.World
{
    public class IsHoldingSensor<T> : LocalWorldSensorBase
        where T : IHoldable
    {
        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent)
        {
            var inventory = agent.GetComponent<ComplexInventoryBehaviour>();
            
            if (inventory == null)
                return false;

            return inventory.Count<T>();
        }
    }
}