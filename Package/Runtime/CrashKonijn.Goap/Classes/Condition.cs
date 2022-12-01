using System;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    [Serializable]
    public class Condition : ICondition
    {
        public IWorldKey worldKey;
        public bool positive = true;
    }
}