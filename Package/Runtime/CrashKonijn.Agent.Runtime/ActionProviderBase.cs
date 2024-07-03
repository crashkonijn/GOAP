using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class ActionProviderBase : MonoBehaviour, IActionProvider
    {
        public abstract IActionReceiver Receiver { get; set; }
        public abstract void ResolveAction();
    }
}