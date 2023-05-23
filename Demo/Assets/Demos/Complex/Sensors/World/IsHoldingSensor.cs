using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Sensors.World
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

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var inventory = references.GetCachedComponent<ComplexInventoryBehaviour>();
            
            if (inventory == null)
                return false;

            return inventory.Count<T>();
        }
    }
}