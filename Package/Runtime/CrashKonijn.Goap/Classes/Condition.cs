using System;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    [Serializable]
    public class Condition : ICondition
    {
        public WorldKey worldKey;
        public bool positive = true;
    }
}