using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use your own implementation of ITargetKey instead!")]
    public class TargetKey : ITargetKey
    {
        public TargetKey(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
