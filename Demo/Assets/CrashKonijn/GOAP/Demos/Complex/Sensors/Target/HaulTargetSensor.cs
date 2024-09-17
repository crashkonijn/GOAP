using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Goap;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.Target
{
    public class HaulTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private ItemCollection itemCollection;

        public void Inject(GoapInjector injector)
        {
            this.itemCollection = injector.itemCollection;
        }
        
        public override void Created()
        {
            
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var item = this.itemCollection.Closest(agent.Transform.position, false, false, agent.Transform.gameObject);
            
            if (item is null)
                return default;
            
            // Re-use the current ItemTarget if it exists
            if (target is ItemTarget itemTarget)
                return itemTarget.SetItem(item);

            return new ItemTarget(item);
        }
    }
}