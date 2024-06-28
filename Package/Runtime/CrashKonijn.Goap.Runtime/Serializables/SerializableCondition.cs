using System;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
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