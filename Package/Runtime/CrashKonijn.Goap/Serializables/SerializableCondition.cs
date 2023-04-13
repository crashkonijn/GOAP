using System;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Scriptables;
using UnityEngine;

namespace CrashKonijn.Goap.Serializables
{
    [Serializable]
    public class SerializableCondition : ICondition
    {
        public WorldKeyScriptable worldKey;

        public IWorldKey WorldKey => this.worldKey;
        
        [field:SerializeField]
        public Comparison Comparison { get; set; }
        
        [field:SerializeField]
        public int Amount { get; set; }
    }
}