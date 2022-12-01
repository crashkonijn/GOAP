using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Serializables
{
    [Serializable]
    public class SerializableCondition : ICondition
    {
        public WorldKeyScriptable worldKey;
        public bool positive = true;

        public IWorldKey WorldKey => this.worldKey;
        public bool Positive => this.positive;
    }
}