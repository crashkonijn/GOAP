using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.World
{
    public class IsHoldingSensor<T> : LocalWorldSensorBase
        where T : IHoldable
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            var inventory = references.GetCachedComponent<ComplexInventoryBehaviour>();
            
            if (inventory == null)
                return false;

            return inventory.Count<T>();
        }
    }
}