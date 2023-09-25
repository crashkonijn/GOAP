using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class InjectBehaviour : MonoBehaviour, IGoapInjector
    {
        [field:SerializeField]
        public GridBehaviour Grid { get; private set; }
        [field:SerializeField]
        public PathfindingBehaviour Pathfinding { get; private set; }

        public void Inject(IActionBase action)
        {
            if (action is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IGoalBase goal)
        {
            if (goal is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IWorldSensor worldSensor)
        {
            if (worldSensor is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(ITargetSensor targetSensor)
        {
            if (targetSensor is IInjectable injectable)
                injectable.Inject(this);
        }
    }
}