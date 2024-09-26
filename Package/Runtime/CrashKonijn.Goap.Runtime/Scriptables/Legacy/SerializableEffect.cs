using System;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    [Obsolete("Use CapabilityConfigs instead!")]
    public class SerializableEffect : IEffect
    {
        public WorldKeyScriptable worldKey;

        public IWorldKey WorldKey => this.worldKey;

        [field: SerializeField]
        public bool Increase { get; set; }
    }
}
