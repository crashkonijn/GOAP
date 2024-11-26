using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class LocalSensor : ILocalSensor
    {
        public Action<IWorldData, IActionReceiver, IComponentReference> SenseMethod;
        public Type Key { get; set; }

        public Type[] GetKeys() => new[] { this.Key };

        public void Created() { }

        public void Update() { }

        public void Sense(IWorldData data, IActionReceiver agent, IComponentReference references) => this.SenseMethod(data, agent, references);
    }
}
