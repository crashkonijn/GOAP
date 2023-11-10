using CrashKonijn.Goap.Core.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Goap
{
    public class GoapInjector : MonoBehaviour, IGoapInjector
    {
        public ItemFactory itemFactory;
        public ItemCollection itemCollection;
        public InstanceHandler instanceHandler;
        
        public void Inject(IAction action)
        {
            if (action is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IGoal goal)
        {
        }

        public void Inject(IWorldSensor worldSensor)
        {
        }

        public void Inject(ITargetSensor targetSensor)
        {
        }
    }
}