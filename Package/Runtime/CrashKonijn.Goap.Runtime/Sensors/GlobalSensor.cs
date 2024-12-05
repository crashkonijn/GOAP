using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GlobalSensor : IGlobalSensor
    {
        public Action<IWorldData> SenseMethod;
        public Type Key { get; set; }

        public void Created() { }

        public Type[] GetKeys() => new[] { this.Key };

        public void Sense(IWorldData data) => this.SenseMethod(data);
    }
}
