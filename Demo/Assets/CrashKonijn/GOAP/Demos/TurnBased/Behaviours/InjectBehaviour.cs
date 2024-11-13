using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class InjectBehaviour : MonoBehaviour, IGoapInjector
    {
        [field: SerializeField]
        public GridBehaviour Grid { get; private set; }

        [field: SerializeField]
        public PathfindingBehaviour Pathfinding { get; private set; }

        public void Inject(IAction action)
        {
            if (action is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IGoal goal)
        {
            if (goal is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(ISensor sensor)
        {
            if (sensor is IInjectable injectable)
                injectable.Inject(this);
        }
    }
}
