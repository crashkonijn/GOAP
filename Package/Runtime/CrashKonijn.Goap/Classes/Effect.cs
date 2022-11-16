using System;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    [Serializable]
    public class Effect : IEffect
    {
        public WorldKey worldKey;
        public bool positive = true;
    }
}