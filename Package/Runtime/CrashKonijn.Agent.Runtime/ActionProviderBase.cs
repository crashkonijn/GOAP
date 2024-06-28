using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class ActionProviderBase : MonoBehaviour, IActionProvider
    {
        public abstract void ResolveAction();
    }
}