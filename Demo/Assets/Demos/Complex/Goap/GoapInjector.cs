using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Goap
{
    public class GoapInjector : MonoBehaviour, IGoapInjector
    {
        public ItemFactory itemFactory;
        
        public void Inject(IActionBase action)
        {
            if (action is IInjectable injectableAction)
                injectableAction.Inject(this);
        }

        public void Inject(IGoalBase action)
        {
        }
    }
}