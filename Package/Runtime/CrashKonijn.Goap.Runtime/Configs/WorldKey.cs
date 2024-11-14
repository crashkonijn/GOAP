using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use your own implementation of IWorldKey instead!")]
    public class WorldKey : IWorldKey
    {
        public WorldKey(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
