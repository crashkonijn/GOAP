using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.World
{
    [GoapId("Simple-HasAppleSensor")]
    public class HasAppleSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var inventory = references.GetCachedComponent<InventoryBehaviour>();

            if (inventory == null)
                return false;

            return inventory.Apples.Count > 0;
        }
    }
}