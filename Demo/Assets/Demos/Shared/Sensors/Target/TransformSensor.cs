using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace Demos.Shared.Sensors.Target
{
    public class TransformSensor : LocalTargetSensorBase
    {
        public override void Update()
        {
            
        }

        public override ITarget Sense(IMonoAgent agent)
        {
            return new TransformTarget(agent.transform);
        }
    }
}