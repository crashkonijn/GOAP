using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Sensors;
using Demos.Simple.Behaviours;

namespace Demos.Simple.Sensors.World
{
    public class HasAppleSensor : LocalWorldSensorBase
    {
        public override void Update()
        {
            
        }

        public override bool Sense(IMonoAgent agent)
        {
            var inventory = agent.GetComponent<InventoryBehaviour>();

            if (inventory == null)
                return false;

            return inventory.Apples.Count > 0;
        }
    }
}