using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class ActionProviderBase : MonoBehaviour, IActionProvider
    {
        private Dictionary<IAction, IActionDisabler> disablers = new();

        public abstract IActionReceiver Receiver { get; set; }
        public abstract void ResolveAction();

        public bool IsDisabled(IAction action)
        {
            if (!this.disablers.TryGetValue(action, out var disabler))
                return false;

            if (this.Receiver is not IMonoAgent agent)
                return false;

            if (disabler.IsDisabled(agent))
                return true;

            this.Enable(action);
            return false;
        }

        public void Enable(IAction action)
        {
            this.disablers.Remove(action);
        }

        public void Disable(IAction action, IActionDisabler disabler)
        {
            this.disablers[action] = disabler;
        }
    }
}
