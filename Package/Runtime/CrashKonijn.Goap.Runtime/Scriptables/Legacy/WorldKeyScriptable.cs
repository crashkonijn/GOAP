using System;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use CapabilityConfigs instead!")]
    public class WorldKeyScriptable : ScriptableObject, IWorldKey
    {
        public string Name => this.name;
    }
}