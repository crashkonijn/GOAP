using System;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using UnityEngine;

namespace CrashKonijn.Goap.Serializables
{
    [Serializable]
    public class SerializableEffect : IEffect
    {
        public WorldKeyScriptable worldKey;

        public IWorldKey WorldKey => this.worldKey;
        
        [field:SerializeField]
        public bool Increase { get; set; }
    }
}