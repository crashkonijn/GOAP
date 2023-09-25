using CrashKonijn.Goap.Classes;
using CrashKonijn.GOAP.Demos.TurnBased.Behaviours;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Sensors.Target
{
    public class TurnWanderTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private IGrid grid;

        public override void Created()
        {
        }

        public void Inject(InjectBehaviour data)
        {
            Debug.Log("Inject");
            this.grid = data.Grid;
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var tile = this.grid.GetWalkableTiles().Random();

            return new TileTarget(tile);
        }
    }
}