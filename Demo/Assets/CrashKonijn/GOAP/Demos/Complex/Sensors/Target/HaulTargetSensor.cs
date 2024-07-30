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

        public override ITarget Sense(IActionReceiver agent, IComponentReference references)
        {
            var item = this.itemCollection.Closest(agent.Transform.position, false, false, agent.Transform.gameObject);
            
            if (item is null)
                return default;

            return new ItemTarget(item);
        }
    }
}