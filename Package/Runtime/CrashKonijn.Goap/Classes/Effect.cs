using System;
using CrashKonijn.Goap.Configs.Interfaces;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    [Serializable]
    public class Effect : IEffect
    {
        public IWorldKey worldKey;
        public bool positive = true;
    }
}