using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Agent
{
    public abstract class ActionProviderBase : MonoBehaviour, IActionProvider
    {
        public abstract void ResolveAction();
    }
}